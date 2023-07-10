using APIServer;
using APIServer.Services;
using APIServer.Middleware;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IAccountDb, AccountDb>();
builder.Services.AddSingleton<IMemoryDb, RedisDb>();
builder.Services.AddControllers();

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
LogManager.SetLoggerFactory(loggerFactory, "Global");

app.UseMiddleware<CheckUserAuth>();

app.UseRouting();

app.MapControllers();
//app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

var redisDb = app.Services.GetRequiredService<IMemoryDb>();
var redisAddress = configuration.GetSection("DbConfig")["Redis"];
if (string.IsNullOrWhiteSpace(redisAddress))
{
    throw new Exception("Redis Address Is Not Defined In The Configuration");
}

redisDb.Init(redisAddress);

app.Run();