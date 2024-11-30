using Microsoft.EntityFrameworkCore;
using Rest.Controllers;
using Rest.Infraestructure;
using Rest.Repositories;
using Rest.Services;
using Rest.Cache;
using StackExchange.Redis;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Configurar el puerto en el que la aplicación escuchará
builder.WebHost.UseUrls("http://*:4000");

// El resto de la configuración se mantiene igual
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IMongoClient, MongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("MongoDb:Persona:ConnectionString")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPerRepository, PerRepository>();

builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => {
    var configuration = builder.Configuration.GetValue<string>("REDIS_HOST") + ":" +
                        builder.Configuration.GetValue<string>("REDIS_PORT");
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}
app.Run();
