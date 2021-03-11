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
            //ע�뷽ʽ����ΪAutofac
            //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            builder.ConfigureWebHostDefaults(webBuilder =>
         {
             webBuilder.UseStartup<Startup>();
         });

            #region �����ļ�
            builder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            {
                //��ʼ������
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
            }).UseNLog();//���У�UseNLog����չ��������Ҫ����NLog.Web.AspNetCore;
            #endregion

            return builder;
        }

    }
}
