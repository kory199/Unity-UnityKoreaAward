using GameAPIServer;
using GameAPIServer.Services;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddSingleton<IVerDb, VerDb>();
builder.Services.AddTransient<IAccountDb, AccountDb>();
builder.Services.AddTransient<IGameDb, GameDb>();
builder.Services.AddTransient<IUserItemDb, UserItemDb>(); 
builder.Services.AddTransient<IVersionDb, VersionDb>(); 
builder.Services.AddTransient<IMailDb, MailDb>(); 
builder.Services.AddTransient<IPayInAppDb, PayInAppDb>(); 
builder.Services.AddTransient<IAttendanceDb, AttendanceDb>();
builder.Services.AddTransient<IDungeonStage, DungeonStageDb>();
builder.Services.AddSingleton<IMasterDataDb, MasterDataDb>();
builder.Services.AddSingleton<IMemoryDb,RedisDb>();

builder.Services.AddControllers();

//SetLogger.SettingLogger(builder, configuration);
Receipt.NewReceipt();

var app = builder.Build();


var masterDb = app.Services.GetRequiredService<IMasterDataDb>();
masterDb.LoadMasterDataAsync();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
LogManager.SetLoggerFactory(loggerFactory, "Global");

app.UseMiddleware<GameAPIServer.Middleware.CheckUserAuth>();

app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

var redisDb = app.Services.GetRequiredService<IMemoryDb>();
redisDb.Init(configuration.GetSection("DbConfig")["Redis"]);
redisDb.SaveNotice();

app.Run();
