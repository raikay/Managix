using Managix.API.Common;
using Managix.Services;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

var hostBuilder = builder.Host;

//�����ļ�
hostBuilder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
{
    //��ʼ������
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
}).UseNLog();//���У�UseNLog����չ��������Ҫ����NLog.Web.AspNetCore;

var logger = NLogBuilder.ConfigureNLog("./Configs/nlog.config").GetCurrentClassLogger();


//��ʼ�������ļ�
builder.Services.AddConfigs(builder.Configuration);
//jwt
builder.Services.AddJwt();
//����ע��
builder.Services.AddContainer();
//����
builder.Services.AddCaChe();
//Swagger
builder.Services.AddSwagger();
//����������
builder.Services.SetController();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
Service.BaseServiceProvider = app.Services;

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
//��֤
app.UseAuthentication();
//��Ȩ
app.UseAuthorization();
//��̬�ļ�
app.UseStaticFilesMiddle();
//Swagger�ĵ�
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerMiddle();
}
app.MapControllers();

app.Run();
