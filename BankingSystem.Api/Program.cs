using AutoMapper;
using BankingSystem.Api.Data;
using BankingSystem.Api.Mapping;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("BankingSystem");
builder.Services.AddSqlite<BankingSystemContext>(connString);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
