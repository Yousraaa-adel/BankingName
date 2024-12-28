using BankingSystem.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("BankingSystem");
builder.Services.AddSqlite<BankingSystemContext>(connString);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
