using Managix.Infrastructure.Authentication;
using Managix.Infrastructure.Configuration;
using Managix.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Autofac;
using Managix.Infrastructure;
using Managix.Infrastructure.Filters;
using Managix.Infrastructure.Helper;
using Managix.Repository.Implement;
using Managix.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;
using System.IO;
using Managix.API.Attributes;
using Managix.API.Filters;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Managix.API.Common
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static class ServicesExtensions
    {
        private static string basePath => AppContext.BaseDirectory;
        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            //初始化配置
            Configs.Init(configuration);
        }


        /// <summary>
        /// 添加认证授权
        /// </summary>
        /// <param name="services"></param>
        public static void AddJwt(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
                options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configs.JwtConfig.Issuer,
                    ValidAudience = Configs.JwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configs.JwtConfig.SecurityKey)),
                    ClockSkew = TimeSpan.Zero//时钟偏移
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });
        }

        #region 容器注入
        /// <summary>
        /// 容器注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddContainer(this IServiceCollection services)
        {

            //用户信息 (Singleton)
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUser, CurrentUser>();

            // Mapper
            services.TryAddSingleton<IMapperService, MapperService>();
            //Sqlite 注入容器
            services.AddDbContext<SqliteContext>(ServiceLifetime.Scoped);

            //DI自动注册仓储、服务
            AddAssembly(services, "Managix.Services");
            AddAssembly(services, "Managix.Repository");
            //services.AddScoped(typeof(ILogger), typeof(Logger));

            //动态注入 仓促，可以不用每个表都创建对用仓储层
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<UploadHelper>();
            //services.AddSingleton<ILogger>(LogManager.GetCurrentClassLogger());
            //（不推荐）ILogger注入，可以不用泛型ILogger<anyClass>这样输入当前类名，输入类名可以可以准确定位日志位置   
            //services.AddSingleton<ILogger>(svc => svc.GetRequiredService<ILogger<GlobalExceptionFilter>>()); 
            //注入 权限处理
            //services.AddScoped<IPermissionHandler, PermissionHandler>();

            //操作日志
            //services.AddSingleton<ILogHandler, LogHandler>();
            
        }

        /// <summary>  
        /// 自动注册服务——获取程序集中的实现类对应的多个接口
        /// </summary>
        /// <param name="services">服务集合</param>  
        /// <param name="assemblyName">程序集名称</param>
        public static void AddAssembly(IServiceCollection services, string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType).ToList();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {

                    var interfaceType = item.GetInterfaces();
                    if (interfaceType.Length == 1)
                    {
                        services.AddTransient(interfaceType[0], item);
                    }
                    if (interfaceType.Length > 1)
                    {
                        services.AddTransient(interfaceType[1], item);
                    }
                }

            }
        }

        #endregion

        #region 缓存
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="services"></param>
        public static void AddCaChe(this IServiceCollection services)
        {
            
            //var cacheConfig = _configHelper.Get<CacheConfig>("cacheconfig", _env.EnvironmentName);
            if (Configs.CacheConfig.Type == CacheType.Redis)
            {
                var csredis = new CSRedis.CSRedisClient(Configs.CacheConfig.Redis.ConnectionString);
                RedisHelper.Initialization(csredis);
                services.AddSingleton<ICache, RedisCache>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<ICache, MemoryCache>();
            }
            
        }
        #endregion

        #region Swagger Api文档
        /// <summary>
        /// 添加 Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            
            // if (_env.IsDevelopment() || _appConfig.Swagger)
            // {
            services.AddSwaggerGen(options =>
            {
                //typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                //{
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "1.0",
                    Title = "SwaggerDemo API",
                    Description = "SwaggerDemo文档",
                });
                //c.OrderActionsBy(o => o.RelativePath);
                // });

                options.IncludeXmlComments(Path.Combine(basePath, "Managix.API.xml"), true);

                options.IncludeXmlComments(Path.Combine(basePath, "Managix.Common.xml"), true);

                options.IncludeXmlComments(Path.Combine(basePath, "Managix.IServices.xml"));

                #region 添加设置Token的按钮

                if (Configs.AppSettings.IdentityServer.Enable)
                {
                    //添加Jwt验证设置
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "oauth2",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                    //统一认证
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Description = "oauth2登录授权",
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{Configs.AppSettings.IdentityServer.Url}/connect/authorize"),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "admin.server.api", "admin后端api" }
                                }
                            }
                        }
                    });
                }
                else
                {
                    //添加Jwt验证设置
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Value: Bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                }

                #endregion
            });
            // }
            
        }
        #endregion

        #region 控制器
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="services"></param>
        public static void SetController(this IServiceCollection services)
        {
            //关闭自带的参数验证
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options =>
            {
                //注册 是否有效值过滤器
                options.Filters.Add<ValiderActionFilter>();
                //注册 全局异常处理
                //options.Filters.Add<GlobalExceptionFilter>();
                //禁止去除ActionAsync后缀
                options.SuppressAsyncSuffixInActionNames = false;
                //全局 启用登录认证 AllowAnonymous属性除外
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());

                //全局启用权限 AllowAnonymous属性除外
                options.Filters.Add(new PermissionAttribute());

                //添加全局操作日志过滤器
                options.Filters.Add<LogActionFilter>();
            })
            

            #region newtonsoft
                //Microsoft.AspNetCore.Mvc.NewtonsoftJson
                //Newtonsoft.Json
                //TODO 改回system.text.json
                .AddNewtonsoftJson(options =>
                {



                    // JsonSerializerDefaults.Web配置：
                    //_propertyNameCaseInsensitive = true; // 不区分大小写的属性匹配
                    //_jsonPropertyNamingPolicy = JsonNamingPolicy.CamelCase; // 对属性名称使用camel大小写
                    //_numberHandling = JsonNumberHandling.AllowReadingFromString; // 允许或写入带引号的数字

                    // 忽略空值
                   // IgnoreNullValues = true,
                // HTML不转义
               // Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping


                    //修改属性名称的序列化方式，首字母小写
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                    //修改时间的序列化方式
                    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                });
            #endregion
        }
        #endregion


    }
}
