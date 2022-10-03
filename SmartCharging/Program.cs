using Microsoft.EntityFrameworkCore;
using SmartCharging;
using SmartCharging.DBContext;
using SmartCharging.Interfaces;
using SmartCharging.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ChargingContext>(opt => opt.UseInMemoryDatabase("Charging"));
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IConnectorService, ConnectorService>();
builder.Services.AddScoped<ICommonService, CommonService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

SeedData(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<DataSeeder>();
        service.Seed();
    }
}