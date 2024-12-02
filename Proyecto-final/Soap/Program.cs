using MongoDB.Driver;
using Soap.Repositories;
using Soap.Services;
using Soap.Contracts;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("ConnectionString__DefaultConnection") 
                     ?? Environment.GetEnvironmentVariable("ConnectionString__DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string is not configured.");
}
builder.Services.AddSingleton<IMongoClient, MongoClient>(s => new MongoClient(connectionString));

builder.Services.AddScoped<IPerRepository, PerRepository>();
builder.Services.AddScoped<IPerContract, PerService>();

// Configura y ejecuta la aplicación
var app = builder.Build();

app.UseSoapEndpoint<IPerContract>("/PerService.svc", new SoapEncoderOptions());

app.Run();
