using MongoDB.Driver;
using Soap.Repositories;
using Soap.Services;
using Soap.Contracts;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

// Intenta obtener la cadena de conexión de las variables de entorno primero
var connectionString = Environment.GetEnvironmentVariable("ConnectionString__DefaultConnection");

// Si no se encuentra, obtén el valor desde appsettings.json
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetSection("MongoDb:Persona:ConnectionString").Value;
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentNullException("La cadena de conexión de MongoDB no puede estar vacía.");
}

Console.WriteLine($"Usando cadena de conexión: {connectionString}");

// Configura el cliente de MongoDB
builder.Services.AddSingleton<IMongoClient, MongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("MongoDb:Persona:ConnectionString")));

// Registra repositorios y servicios
builder.Services.AddScoped<IPerRepository, PerRepository>();
builder.Services.AddScoped<IPerContract, PerService>();

// Configura y ejecuta la aplicación
var app = builder.Build();

app.UseSoapEndpoint<IPerContract>("/PerService.svc", new SoapEncoderOptions());

app.Run();
