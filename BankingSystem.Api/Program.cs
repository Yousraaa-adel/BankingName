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

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
  {
    Title = "Banking System API",
    Version = "v1",
    Description = "API documentation for the Banking System project."
  });

  // Include XML comments if generated
  var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
  options.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

// Enable Swagger middleware for API documentation
if (app.Environment.IsDevelopment())
{
  app.UseSwagger(); // Generate Swagger JSON
  app.UseSwaggerUI(); // Serve Swagger UI at /swa gger
}

app.MapGet("/", () => "Hello World!");

app.MapAccountEndpoints();

app.Run();
