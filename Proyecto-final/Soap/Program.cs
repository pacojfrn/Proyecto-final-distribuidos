using MongoDB.Driver;
using Soap.Contracts;
using Soap.Repositories;
using Soap.Contracts.Services;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();
builder.Services.AddScoped<IPersonaRepository, PersonaRespository>();
builder.Services.AddScoped<IPersonaContract, PersonaService>();


builder.Services.AddSingleton<IMongoClient, MongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("MongoDb:Groups:ConnectionString")));

var app = builder.Build();
app.UseSoapEndpoint<IPersonaContract>("/PersonaService.svc", new SoapEncoderOptions());


app.Run();