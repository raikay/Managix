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
        /// ���캯��
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
            //��ʼ�������ļ�
            services.AddConfigs(Configuration);
            //jwt
            services.AddJwt();
            //����ע��
            services.AddContainer();
            //����
            services.AddCaChe();
            //Swagger
            services.AddSwagger();
            //����������
            services.SetController();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Service����
            Service.BaseServiceProvider = app.ApplicationServices;
            if (env.IsDevelopment())
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

            // Swagger�ĵ�
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
