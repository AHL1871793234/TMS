﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ApplicationInsights.OwinExtensions;
using Microsoft.AspNetCore.HttpsPolicy;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using System.Threading.Tasks;
using TMS.Repository.User;
using System.Reflection;
using TMS.Service.User;
using TMS.Common.Redis;
using TMS.Common.Log;
using TMS.Common.Jwt;
using TMS.Common.DB;
using IdentityModel;
using System.Linq;
using System.Text;
using System.IO;
using Autofac;
using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

namespace TMS.API
{
    /// <summary>
    /// Startup 类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// IConfiguration
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            #region 注入Dapper配置
            //连接SqlServer数据库
            services.AddDapper("SqlDb", m =>
            {
                m.ConnectionString = Configuration.GetConnectionString("SqlServer");
                //数据库类型SqlServer
                m.DbType = DbStoreType.SqlServer;
            });
            #endregion

            #region Redis缓存
            ////redis缓存
            //var section = Configuration.GetSection("Redis:Default");
            ////连接字符串
            //ConfigHelperRedis._conn = section.GetSection("Connection").Value;
            ////实例化名称
            //ConfigHelperRedis._name = section.GetSection("InstanceName").Value;
            ////密码
            //ConfigHelperRedis._pwd = section.GetSection("PassWord").Value;
            ////默认数据库
            //ConfigHelperRedis._db = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
            ////端口号
            //ConfigHelperRedis._port = int.Parse(section.GetSection("Prot").Value);
            ////服务器名称/IP
            //ConfigHelperRedis._server = section.GetSection("Server").Value;

            //services.AddSingleton(new RedisHelper());
            #endregion

            #region SQL注入
            //控制器上加SQL注入过滤器
            //services.AddControllers(options =>
            //{
            //    options.Filters.Add<CustomSQLInjectFilter>();
            //});
            //services.AddControllers();
            #endregion

            #region JWT验证配置
            //JWT优点
            //通用：因为json的通用性，所以JWT是可以进行跨语言支持的，像JAVA,JavaScript,NodeJS,PHP等很多语言都可以使用。
            //紧凑：JWT的构成非常简单，字节占用很小，可以通过 GET、POST 等放在 HTTP 的 header 中，非常便于传输。
            //扩展：JWT是自我包涵的，包含了必要的所有信息，不需要在服务端保存会话信息, 非常易于应用的扩展。

            //获取JWT配置
            var jwtTokenConfig = Configuration.GetSection("JWTConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);
            
            //注册JwtTokenConfig配置服务
            services.Configure<JwtTokenConfig>(Configuration.GetSection("JWTConfig"));
            services.AddTransient<ITokenHelper, TokenHelper>();
            //配置身份认证服务 认证参数
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//认证模式
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//质询模式
            })
            .AddJwtBearer(x =>
            {
                //对JwtBearer进行配置
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    //Token验证参数         
                    ValidateIssuer = true,//是否验证Issuer
                    //是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数            
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    //是否验证签名,不验证的画可以篡改数据，不安全 在Configure方法添加认证方法   4、生成Jwt的Token令牌           
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.IssuerSigningKey)),
                    //发行人            
                    ValidateAudience = true,
                    //是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数            
                    ValidAudience = jwtTokenConfig.Audience,
                    //订阅人  
                    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比         
                    ValidateLifetime = true,
                    //是否验证过期时间，过期了就拒绝访问            
                    //这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0            
                    ClockSkew = TimeSpan.FromMinutes(1)  //对token过期时间验证的允许时间
                };
            });
            #endregion

            #region Swagger(斯瓦格)验证及配置
            //ASP.NET Core MVC 的兼容性版本配置
            //CompatibilityVersion 值 Version_2_0 到 Version_2_2 被标记为[Obsolete(...)]。
            //对于 ASP.NET Core 3.0，已删除兼容性开关支持的旧行为
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //如果将应用程序的兼容性版本设置为 Version_2_0 则此设置的值将为 false ，除非进行显式配置。
            //如果将应用程序的兼容性版本设置为 Version_2_1 或更高版本，则除非显式配置，否则此设置的值将 为 true 。
            //.AddMvcOptions(options =>
            //{
            //    // Don't combine authorize filters (keep 2.0 behavior).
            //    options.AllowCombiningAuthorizeFilters = false;
            //    // All exceptions thrown by an IInputFormatter are treated
            //    // as model state errors (keep 2.0 behavior).
            //    options.InputFormatterExceptionPolicy =
            //        InputFormatterExceptionPolicy.AllExceptions;
            //});
            //添加并配置 Swagger 中间件
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                //标题—版本—描述
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TMS.API", Version = "v1", Description = "TMS.API" });
                //为Swagger 设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //API控制器层注释，true表示显示控制器注释
                c.IncludeXmlComments(xmlPath, true);

                //开启权限小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                
                //开启Authorize权限按钮
                c.AddSecurityDefinition("JWTBearer", new OpenApiSecurityScheme()
                {
                    Description = "这是方式一(直接在输入框中输入认证信息，不需要在开头添加Bearer) ",
                    Name = "Authorization",         //jwt默认的参数名称
                    In = ParameterLocation.Header,  //jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                //定义JwtBearer认证方式二
                //options.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
                //{
                //    Description = "这是方式二(JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）)",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.ApiKey
                //});

                //声明一个Scheme，注意下面的Id要和上面AddSecurityDefinition中的参数name一致
                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "JWTBearer",   //这个名字与上面的一样
                        Type = ReferenceType.SecurityScheme
                    }
                };
                //注册全局认证（所有的接口都可以使用认证）
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    { scheme, Array.Empty<string>() }
                //});

            });
            #endregion

            #region 跨域
            //添加cors 服务 配置跨域来处理
            services.AddCors(options => options.AddPolicy("cor",
            builder =>
            {
                builder.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials();
            }));
            #endregion
        }
        /// <summary>
        /// 使用此方法配置HTTP请求管道。
        /// </summary>
        /// <param name="app">IApplicationBuilder 定义用于配置应用请求管道的类，ASP.NET Core 请求管道包含一系列请求委托，依次调用。</param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            #region 配置HTTP请求管道
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //HTTPS 重定向中间件 (UseHttpsRedirection) 将 HTTP 请求重定向到 HTTPS。
            app.UseHttpsRedirection();
            //静态文件中间件(UseStaticFiles) 返回静态文件，并简化进一步请求处理。
            app.UseStaticFiles();
            //用于路由请求的路由中间件 (UseRouting)。
            app.UseRouting();
            //身份验证中间件 (UseAuthentication) 尝试对用户进行身份验证，然后才会允许用户访问安全资源。
            app.UseAuthentication();
            //用于授权用户访问安全资源的授权中间件
            app.UseAuthorization();
            //配置Cors跨域
            app.UseCors("cor");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion

            #region 日志配置
            //添加NLog
            loggerFactory.AddNLog();
            //加载配置
            NLog.LogManager.LoadConfiguration("NLog.config");
            //调用自定义的中间件
            app.UseLog();

            //第二种方法
            //Log4Net日志配置—中间件大同小异
            //loggerFactory.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "log4net.config"));
            #endregion

            #region Swagger 环境
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //启用中间件服务生成Swagger作为JSON终结点
                app.UseSwagger();
                //请用中间件服务对Swagger-UI，指定Swagger JSON终结点
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TMS.API v1");
                });
            }
            #endregion

            #region 添加中间件组件
            if (env.IsDevelopment())
            {
                //开发人员异常页中间件 (UseDeveloperExceptionPage) 报告应用运行时错误：从管道捕获同步和异步Exception实例，并生成HTML错误响应。
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                //异常处理程序中间件 (UseExceptionHandler) 捕获以下中间件中引发的异常。
                app.UseExceptionHandler("/Error");
                //HTTP 严格传输安全协议 (HSTS) 中间件 (UseHsts) 添加 Strict-Transport-Security 标头。
                app.UseHsts();
            }
            #endregion

            #region 异常处理配置 
            //判断是否是开发环境
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//使用异常记录页面
            }
            else
            {
                app.UseExceptionHandler("/VehicleManagementAPI/error"); //在生产环境下，发生系统错误时，跳转到错误页面
            }
            //UseStatusCodePages()支持多种扩展方法。其中一个方法接受一个lambda表达式:
            app.UseStatusCodePages(async context => {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(
                    "Status code page,status code:" + context.HttpContext.Response.StatusCode);
            });//使用HTTP错误代码页
            #endregion
        }

        #region 使用AutoFac 依赖注入
        //ConfigureContainer是您可以直接注册的地方

        //使用AutoFac，他在ConfigureServices之后运行，因此

        //此处将覆盖在ConfigureServices中进行的注册。

        //不要建造容器：工厂会帮你完成的。
        /// <summary>
        /// ConfigureContainer是您可以直接注册的地方
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //在这里直接向AutoFac注册您自己的东西，不要
            //调用builder.Populate(),这发生在AutofacServiceProviderFactory中
            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }
        #endregion
    }
}
