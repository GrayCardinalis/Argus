using Argus.Data;
using Argus.Infrastructure;
using Argus.Mappings;
using Argus.Providers;
using Argus.Providers.Interfaces;
using Argus.Services;
using Argus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Diagnostics;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuditoriumService, AuditoriumService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICurrentUserProvider, FakeCurrentUserProvider>();

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

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
