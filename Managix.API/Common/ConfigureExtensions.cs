using Managix.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managix.API.Common
{
    /// <summary>
    /// Configure
    /// </summary>
    public static class ConfigureExtensions
    {
        /// <summary>
        /// 启用Swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerMiddle(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SwaggerDemo API V1");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                c.DefaultModelsExpandDepth(-1);//不显示Models
            });
        }

        #region 启动静态文件
        /// <summary>
        /// 启用静态文件
        /// </summary>
        /// <param name="app"></param>
        public static void UseStaticFilesMiddle(this IApplicationBuilder app)
        {
            //启动静态文件  根目录 wwwroot
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            // defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("index.html");
            // app.UseDefaultFiles(defaultFilesOptions);

            //  app.UseDefaultFiles();//默认含index.html
            // app.UseStaticFiles();


            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();


            #region 上传文件
            //上传文件的文件夹 配置文件静态文件
            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            var filePath = Configs.UploadConfig.Avatar.UploadPath;
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = new PathString(Configs.UploadConfig.Avatar.RequestPath), //请求地址
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(filePath)//用于定位资源的文件系统

            }); 
            #endregion
        }

        #endregion
    }
}
