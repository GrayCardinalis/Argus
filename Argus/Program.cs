using System.Text;
using Argus.Data;
using Argus.Infrastructure;
using Argus.Mappings;
using Argus.Options;
using Argus.Providers;
using Argus.Providers.Interfaces;
using Argus.Services;
using Argus.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using Argus.Constants.Security;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuditoriumService, AuditoriumService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICurrentUserProvider, JwtCurrentUserProvider>();
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
    limiterOptions.AddPolicy(RateLimitPolicies.Auth, httpContext =>
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

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
});


var jwtOptions = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>()
    ?? throw new InvalidOperationException(
        $"Configuration section '{JwtOptions.SectionName}' is missing");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false; // Disable automatic claim mapping to avoid conflicts with custom claims

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero, // Optional: Set clock skew to zero for precise expiration validation
            
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.Key)),
            NameClaimType = "sub", // Use the "sub" claim as the name identifier
            RoleClaimType = "role" // Use the "role" claim for role-based authorization
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"[AUTH FAIL] {ctx.Exception.GetType().Name}: {ctx.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference().AllowAnonymous();
}

app.UseRouting();

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthentication();

app.Use(async (context, next) =>
{
    var u = context.User;
    var hasAuthHeader = context.Request.Headers.ContainsKey("Authorization");
    Console.WriteLine($"[PIPE] {context.Request.Scheme} authHeader={hasAuthHeader}, IsAuthenticated={u.Identity?.IsAuthenticated}, claims={u.Claims.Count()}");
    await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();

//test del1
//test del2