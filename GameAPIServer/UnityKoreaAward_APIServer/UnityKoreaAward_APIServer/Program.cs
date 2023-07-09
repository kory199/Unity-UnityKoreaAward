using UnityKoreaAward_APIServer.Services;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

var app = builder.Build();

app.UseRouting();

app.Run();