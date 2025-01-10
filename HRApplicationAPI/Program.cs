using AutoMapper;
using HRApplicationAPI.Data;
using HRApplicationAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using HRApplicationAPI.Extensions;
using HRApplicationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbProvider = builder.Configuration.GetConnectionString("Provider");
builder.Services.AddDbContext<DataContext>(options =>
{
    if (dbProvider == "SqlServer")
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionString"));
    }
});

builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddLogging();
builder.Services.AddSingleton(typeof(ILogger), sp => sp.GetRequiredService<ILogger<ControllerBase>>());

var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfiles()));
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddAuthenticationService(builder.Configuration);

builder.Services.AddSwaggerService();

builder.Services.AddScoped<DBSeeder>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<EventService>();

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyHeader())
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSeedDB();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
