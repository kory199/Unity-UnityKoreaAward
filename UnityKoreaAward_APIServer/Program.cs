using UnityKoreaAward_APIServer.Services;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IAccountDb, AccountDb>();

builder.Services.AddControllers();

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();