using Managix.API.Common;
using Managix.Services;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

var hostBuilder = builder.Host;

//配置文件
hostBuilder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
{
    //初始化配置
    configurationBuilder.AddJsonFile("appsettings.json");
    configurationBuilder.AddJsonFile("./Configs/jwtconfig.json");
    configurationBuilder.AddJsonFile("./Configs/dbconfig.json");
    configurationBuilder.AddJsonFile("./Configs/cacheconfig.json");
    configurationBuilder.AddJsonFile("./Configs/uploadconfig.json");
    //configurationBuilder.
});
//NLog
hostBuilder.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    logging.AddConsole();
}).UseNLog();//其中，UseNLog是拓展方法，需要引入NLog.Web.AspNetCore;

var logger = NLogBuilder.ConfigureNLog("./Configs/nlog.config").GetCurrentClassLogger();


//初始化配置文件
builder.Services.AddConfigs(builder.Configuration);
//jwt
builder.Services.AddJwt();
//容器注入
builder.Services.AddContainer();
//缓存
builder.Services.AddCaChe();
//Swagger
builder.Services.AddSwagger();
//控制器配置
builder.Services.SetController();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
Service.BaseServiceProvider = app.Services;

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
//认证
app.UseAuthentication();
//授权
app.UseAuthorization();
//静态文件
app.UseStaticFilesMiddle();
//Swagger文档
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerMiddle();
}
app.MapControllers();

app.Run();
