using AutoMapper;
using BankingSystem.Api.Data;
using BankingSystem.Api.Mapping;
using BankingSystem.Api.Endpoints;
using BankingSystem.Api.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("BankingSystem");
builder.Services.AddSqlite<BankingSystemContext>(connString);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapAccountEndpoints();

app.Run();
