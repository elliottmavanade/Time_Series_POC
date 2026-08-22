using Api.Services;
using Api.Services.Interfaces;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);

// Add services to the container.

builder.Services.AddControllers()
    // Scaffolded Domain.Entities types (e.g. Sensor <-> ScheduledCalc/SensorReading) have navigation
    // properties that form reference cycles; ignore them rather than throwing when serializing responses.
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TimeSeriesPocDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TimeSeriesPoc")));

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ITimeSeriesService, TimeSeriesService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorCalcRelationshipRepository, SensorCalcRelationshipRepository>();
builder.Services.AddScoped<IScheduledCalcsRepository, ScheduledCalcsRepository>();
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
builder.Services.AddScoped<ISensorCategoryRepository, SensorCategoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
