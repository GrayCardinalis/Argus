using Argus.Data;
using Argus.Services;
using Argus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Argus.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

builder.Services.AddScoped<IAuditoriumService, AuditoriumService>();
builder.Services.AddScoped<IComponentService, ComponentService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention());

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AuditoriumMappingProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ComponentMappingProfile>());


var app = builder.Build();

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
