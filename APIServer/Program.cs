using APIServer;
using APIServer.Services;
using APIServer.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IAccountDb, AccountDb>();
builder.Services.AddSingleton<IMemoryDb, RedisDb>();
builder.Services.AddControllers();

// TODO : google 인증 구성
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
LogManager.SetLoggerFactory(loggerFactory, "Global");

app.UseRouting();

app.UseMiddleware<CheckUserAuth>();

// 미들웨어
app.UseAuthentication();
app.UseAuthorization();

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