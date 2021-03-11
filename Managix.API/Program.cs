using Autofac.Extensions.DependencyInjection;
using Managix.Infrastructure.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;

namespace Managix.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Nlog
            //var logger = LogManager.GetCurrentClassLogger();
            var logger = NLogBuilder.ConfigureNLog("./Configs/nlog.config").GetCurrentClassLogger();
            #endregion
            try
            {
                Console.WriteLine("launching...");
                //Console.WriteLine($"{string.Join("\r\n", appConfig.Urls)}\r\n");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
            }
            finally
            {
                LogManager.Shutdown();
            }


        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            //注入方式更换为Autofac
            //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            builder.ConfigureWebHostDefaults(webBuilder =>
         {
             webBuilder.UseStartup<Startup>();
         });

            #region 配置文件
            builder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            {
                //初始化配置
                configurationBuilder.AddJsonFile("appsettings.json");
                configurationBuilder.AddJsonFile("./Configs/jwtconfig.json");
                configurationBuilder.AddJsonFile("./Configs/dbconfig.json");
                configurationBuilder.AddJsonFile("./Configs/cacheconfig.json");
                configurationBuilder.AddJsonFile("./Configs/uploadconfig.json");
                //configurationBuilder.
            });
            #endregion

            #region Nlog
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                logging.AddConsole();
            }).UseNLog();//其中，UseNLog是拓展方法，需要引入NLog.Web.AspNetCore;
            #endregion

            return builder;
        }

    }
}
