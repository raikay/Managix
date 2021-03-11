using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Managix.Services;
using Managix.API.Common;

namespace Managix.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //初始化配置文件
            services.AddConfigs(Configuration);
            //jwt
            services.AddJwt();
            //容器注入
            services.AddContainer();
            //缓存
            services.AddCaChe();
            //Swagger
            services.AddSwagger();
            //控制器配置
            services.SetController();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Service基类
            Service.BaseServiceProvider = app.ApplicationServices;
            if (env.IsDevelopment())
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

            // Swagger文档
            if (env.IsDevelopment())
            {
                app.UseSwaggerMiddle();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }




    }

}
