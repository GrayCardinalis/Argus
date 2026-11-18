// dotnet run --file scripts/Sprint7Smoke.cs (start the local API with a disposable DB first).
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

var baseUrl = new Uri(Environment.GetEnvironmentVariable("ARGUS_TEST_URL") ?? "http://localhost:5150/api/");
if (!baseUrl.IsLoopback)
    throw new InvalidOperationException("Use a local API connected to a disposable test database.");
using var http = new HttpClient { BaseAddress = baseUrl };
var admin = Environment.GetEnvironmentVariable("ARGUS_ADMIN_TOKEN")
    ?? throw new InvalidOperationException("Set ARGUS_ADMIN_TOKEN to a test admin's access token.");
var suffix = Guid.NewGuid().ToString("N")[..12];

async Task<(int Status, JsonNode? Body)> Send(string method, string path, string? token, object? body = null)
{
    using var request = new HttpRequestMessage(new HttpMethod(method), path);
    if (token != null) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    if (body != null) request.Content = JsonContent.Create(body);
    using var response = await http.SendAsync(request);
    var text = await response.Content.ReadAsStringAsync();
    return ((int)response.StatusCode, string.IsNullOrWhiteSpace(text) ? null : JsonNode.Parse(text));
}

async Task<JsonNode?> Expect(int status, string method, string path, string? token, object? body = null)
{
    var result = await Send(method, path, token, body);
    if (result.Status != status || (status >= 400 && (int?)result.Body?["status"] != status))
        throw new InvalidOperationException($"{method} {path}: expected {status}, got {result.Status}. {result.Body?["title"]}");
    return result.Body;
}

void Check(bool ok, string message)
{
    if (!ok) throw new InvalidOperationException(message);
}

async Task<string> Create(string path, string? token, object body) =>
    (string)(await Expect(201, "POST", path, token, body))!["id"]!;

await Expect(401, "GET", "user", null);
await Expect(200, "GET", "user?pageSize=1", admin);
var userName = $"qa{suffix}";
var password = Guid.NewGuid().ToString("N");
var userId = await Create("user", null, new { userName, fullName = userName, email = $"{userName}@example.org", password, confirmPassword = password });
var login = await Expect(200, "POST", "auth/login", null, new { userName, password });
var client = (string)login!["accessToken"]!;
await Expect(403, "GET", "user", client);

var room = await Create("auditorium", admin, new { roomNumber = suffix, buildingNumber = 1 });
var equipment = await Create("equipment", admin, new { inventoryNumber = suffix, modelName = "QA", type = 1 });
var ticket = new { title = new string('T', 150), description = new string('D', 2000), equipmentId = equipment };
await Expect(409, "POST", "supportrequest", client, ticket);
await Expect(204, "POST", $"equipment/{equipment}/move", admin, new { newLocationId = room });
await Expect(400, "POST", "supportrequest", client, new { title = ticket.title + "x", ticket.description, ticket.equipmentId });
await Expect(400, "POST", "supportrequest", client, new { ticket.title, description = ticket.description + "x", ticket.equipmentId });
var own = "supportrequest/" + await Create("supportrequest", client, ticket);
var other = "supportrequest/" + await Create("supportrequest", admin, ticket);
await Expect(403, "GET", other, client);
await Expect(404, "GET", $"supportrequest/{Guid.NewGuid()}", client);
await Expect(201, "POST", own + "/comments", client, new { message = "history" });
await Expect(403, "GET", other + "/comments", client);
await Expect(403, "GET", other + "/components", client);

foreach (var path in new[] { "user", "auditorium", "component", "equipment", "supportrequest", own + "/comments",
    own + "/components", $"equipment/{equipment}/placement-history", $"auditorium/{room}/equipment" })
{
    await Expect(400, "GET", path + "?page=0", admin);
    await Expect(400, "GET", path + "?pageSize=101", admin);
    var empty = await Expect(200, "GET", path + "?page=2147483647&pageSize=100", admin);
    Check(empty!["items"]!.AsArray().Count == 0, $"Expected empty page at {path}");
}

var part = await Create("component", admin, new { name = "stock-" + suffix, quantity = 5 });
var stock = $"component/{part}/stock";
object Consume(int n) => new { componentId = part, quantity = n };
await Expect(204, "POST", own + "/components", admin, Consume(3));
await Expect(409, "POST", own + "/components", admin, Consume(3));
await Expect(409, "PUT", stock, admin, new { expectedQuantity = 5, quantity = 7 });
await Expect(400, "PUT", stock, admin, new { quantity = 7 });
Check((int?)(await Expect(200, "GET", $"component/{part}", admin))!["quantity"] == 2, "Stale edit changed stock.");
await Expect(204, "PUT", stock, admin, new { expectedQuantity = 2, quantity = 1 });
var lastPart = await Task.WhenAll(Send("POST", own + "/components", admin, Consume(1)),
    Send("POST", other + "/components", admin, Consume(1)));
Check(lastPart.Select(x => x.Status).Order().SequenceEqual(new[] { 204, 409 }), "Last part must be consumed once.");
Check((int?)(await Expect(200, "GET", $"component/{part}", admin))!["quantity"] == 0, "Unexpected stock.");
var used = 0;
foreach (var path in new[] { own, other })
{
    var page = await Expect(200, "GET", path + "/components", admin);
    used += page!["items"]!.AsArray().Sum(item => (int)item!["quantity"]!);
}
Check(used == 4, "Incorrect total consumption.");
await Expect(409, "DELETE", $"component/{part}", admin);

var secondRoom = await Create("auditorium", admin, new { roomNumber = "B" + suffix, buildingNumber = 1 });
var move = Expect(204, "POST", $"equipment/{equipment}/move", admin, new { newLocationId = secondRoom });
var createDuringMove = Expect(201, "POST", "supportrequest", client, ticket);
await Task.WhenAll(move, createDuringMove);
var capturedRoom = (string?)(await createDuringMove)!["auditorium"]!["id"];
Check(capturedRoom == room || capturedRoom == secondRoom, "Invalid room during concurrent move.");
var movedTicket = await Expect(201, "POST", "supportrequest", client, ticket);
Check((string?)movedTicket!["auditorium"]!["id"] == secondRoom, "Ticket used an outdated placement.");
var originalTicket = await Expect(200, "GET", own, client);
Check((string?)originalTicket!["auditorium"]!["id"] == room, "Move changed an existing ticket.");

await Expect(204, "DELETE", $"equipment/{equipment}", admin);
await Expect(409, "POST", "supportrequest", client, ticket);
await Expect(204, "DELETE", $"user/{userId}", admin);
await Expect(401, "GET", own, client);
var history = await Expect(200, "GET", own, admin);
var comments = await Expect(200, "GET", own + "/comments", admin);
Check((string?)history!["client"]!["id"] == userId && (string?)comments!["items"]![0]!["authorName"] == userName,
    "Historical author was lost after soft deletion.");
Console.WriteLine("Sprint 7 smoke checks passed.");
