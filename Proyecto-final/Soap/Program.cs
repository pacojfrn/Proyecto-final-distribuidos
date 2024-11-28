using MongoDB.Driver;
using Soap.Repositories;
using Soap.Services;
using Soap.Contracts;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient, MongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("MongoDb:Persona:ConnectionString")));

builder.Services.AddScoped<IPerRepository, PerRepository>();
builder.Services.AddScoped<IPerContract, PerService>();

var app = builder.Build();
app.UseSoapEndpoint<IPerContract>("/PerService.svc", new SoapEncoderOptions());

app.Run();
