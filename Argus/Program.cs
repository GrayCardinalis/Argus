using Argus.Data;
using Argus.Infrastructure;
using Argus.Mappings;
using Argus.Options;
using Argus.Providers;
using Argus.Providers.Interfaces;
using Argus.Services;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuditoriumService, AuditoriumService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICurrentUserProvider, FakeCurrentUserProvider>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseSnakeCaseNamingConvention());

builder.Services.AddAutoMapper(cfg =>
{ 
    cfg.AddProfile<AuditoriumMappingProfile>();
    cfg.AddProfile<ComponentMappingProfile>();
    cfg.AddProfile<UserMappingProfile>();
});

builder.Services.AddExceptionHandler<UniqueConstraintExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddRateLimiter(limiterOptions =>
{
    limiterOptions.AddPolicy("login", httpContext =>
    RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        factory: _ => new FixedWindowRateLimiterOptions
        {
            Window = TimeSpan.FromMinutes(1),
            PermitLimit = 100,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0 // No queuing, reject immediately if limit is exceeded
        })
    );

    limiterOptions.OnRejected = async (context, cancellationToken) =>
    {
        if(context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter = ((int) retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
        }
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        var problemDetailsService = context.HttpContext.RequestServices.GetRequiredService<IProblemDetailsService>();

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context.HttpContext,
            ProblemDetails = new ProblemDetails
            {
                Title = "Too Many Requests",
                Status = StatusCodes.Status429TooManyRequests,
                Detail = "Rate limit exceeded. Please try again later.",
                Type = "https://tools.ietf.org/html/rfc6585#section-4"
            }
        });
    };
});

builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart(); //переносит проверку на момент запуска. По умолчанию валидация ленивая — срабатывает при первом обращении к IOptions<JwtOptions>.Value.

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRouting();

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
