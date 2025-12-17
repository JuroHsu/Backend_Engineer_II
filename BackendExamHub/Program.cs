
using BackendExamHub.Endpoints;
using BackendExamHub.Services;
using DotNetEnv;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_SERVER")};" +
                      $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                      $"User Id={Environment.GetEnvironmentVariable("DB_USER")};" +
                      $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                      $"Encrypt={Environment.GetEnvironmentVariable("DB_ENCRYPT")};" +
                      $"TrustServerCertificate={Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE")};";

builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

builder.Services.AddScoped<MyofficeServices>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapMyofficeEndpoints();
app.Run();
