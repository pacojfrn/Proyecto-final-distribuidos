using MongoDB.Driver;
using Soap.Repositories;
using Soap.Services;
using Soap.Contracts;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = Environment.GetEnvironmentVariable("ConnectionString__DefaultConnection");
builder.Services.AddSingleton<IMongoClient, MongoClient>(s => new MongoClient(connectionString));

// Registra repositorios y servicios
builder.Services.AddScoped<IPerRepository, PerRepository>();
builder.Services.AddScoped<IPerContract, PerService>();

// Configura y ejecuta la aplicación
var app = builder.Build();

app.UseSoapEndpoint<IPerContract>("/PerService.svc", new SoapEncoderOptions());

app.Run();
