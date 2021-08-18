using Microsoft.AspNetCore.Authentication.JwtBearer;
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

namespace TMS.API
{
    /// <summary>
    /// Startup ��
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

            #region ע��Dapper����
            //����SqlServer���ݿ�
            services.AddDapper("SqlDb", m =>
            {
                m.ConnectionString = Configuration.GetConnectionString("SqlServer");
                //���ݿ�����SqlServer
                m.DbType = DbStoreType.SqlServer;
            });
            #endregion

            #region Redis����
            ////redis����
            //var section = Configuration.GetSection("Redis:Default");
            ////�����ַ���
            //ConfigHelperRedis._conn = section.GetSection("Connection").Value;
            ////ʵ��������
            //ConfigHelperRedis._name = section.GetSection("InstanceName").Value;
            ////����
            //ConfigHelperRedis._pwd = section.GetSection("PassWord").Value;
            ////Ĭ�����ݿ�
            //ConfigHelperRedis._db = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
            ////�˿ں�
            //ConfigHelperRedis._port = int.Parse(section.GetSection("Prot").Value);
            ////����������/IP
            //ConfigHelperRedis._server = section.GetSection("Server").Value;

            //services.AddSingleton(new RedisHelper());
            #endregion

            #region SQLע��
            //�������ϼ�SQLע�������
            //services.AddControllers(options =>
            //{
            //    options.Filters.Add<CustomSQLInjectFilter>();
            //});
            //services.AddControllers();
            #endregion

            #region Swagger(˹�߸�)��֤������
            services.AddControllers();
            //ASP.NET Core MVC �ļ����԰汾����
            //CompatibilityVersion ֵ Version_2_0 �� Version_2_2 �����Ϊ[Obsolete(...)]��
            //���� ASP.NET Core 3.0����ɾ�������Կ���֧�ֵľ���Ϊ
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //�����Ӧ�ó���ļ����԰汾����Ϊ Version_2_0 ������õ�ֵ��Ϊ false �����ǽ�����ʽ���á�
            //�����Ӧ�ó���ļ����԰汾����Ϊ Version_2_1 ����߰汾���������ʽ���ã���������õ�ֵ�� Ϊ true ��
            //.AddMvcOptions(options =>
            //{
            //    // Don't combine authorize filters (keep 2.0 behavior).
            //    options.AllowCombiningAuthorizeFilters = false;
            //    // All exceptions thrown by an IInputFormatter are treated
            //    // as model state errors (keep 2.0 behavior).
            //    options.InputFormatterExceptionPolicy =
            //        InputFormatterExceptionPolicy.AllExceptions;
            //});
            //��Ӳ����� Swagger �м��
            //ע��Swagger������������һ���Ͷ��Swagger �ĵ�
            services.AddSwaggerGen(c =>
            {
                //���⡪�汾������
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TMS.API", Version = "v1", Description = "TMS.API" });
                //ΪSwagger ����xml�ĵ�ע��·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //API��������ע�ͣ�true��ʾ��ʾ������ע��
                c.IncludeXmlComments(xmlPath, true);
            });
            #endregion

            #region JWT��֤����
            //JWT�ŵ�
            //ͨ�ã���Ϊjson��ͨ���ԣ�����JWT�ǿ��Խ��п�����֧�ֵģ���JAVA,JavaScript,NodeJS,PHP�Ⱥܶ����Զ�����ʹ�á�
            //���գ�JWT�Ĺ��ɷǳ��򵥣��ֽ�ռ�ú�С������ͨ�� GET��POST �ȷ��� HTTP �� header �У��ǳ����ڴ��䡣
            //��չ��JWT�����Ұ����ģ������˱�Ҫ��������Ϣ������Ҫ�ڷ���˱���Ự��Ϣ, �ǳ�����Ӧ�õ���չ��

            //��ȡJWT����
            var jwtTokenConfig = Configuration.GetSection("JWTConfig").Get<JwtTokenConfig>();

            //ע��JwtTokenConfig���÷���
            services.Configure<JwtTokenConfig>(Configuration.GetSection("JWTConfig"));

            //Ȩ�أ�
            //����������������
            //AddTransient �����ȡ-��GC����-�����ͷţ� ÿһ�λ�ȡ�Ķ��󶼲���ͬһ��
            //AddSingleton��Ŀ����-��Ŀ�ر� �൱�ھ�̬�� ֻ����һ��
            //AddScoped����ʼ-�������  ����������л�ȡ�Ķ�����ͬһ�� 
            services.AddTransient<ITokenHelper, TokenHelper>();

            //���������֤����
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//��֤ģʽ
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//��ѯģʽ
            })
            .AddJwtBearer(x =>
            {
                //��JwtBearer��������
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    //NameClaimType = JwtClaimTypes.Name,
                    //RoleClaimType = JwtClaimTypes.Role,
                    //ValidIssuer = "http://localhost:5200",
                    //ValidAudience = "api",
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Consts.Secret))

                    //======================TokenValidationParameters�Ĳ���Ĭ��ֵ=======================
                    RequireSignedTokens = true,
                    SaveSigninToken = false,
                    ValidateActor = false,
                    //������������������Ϊfalse�����Բ���֤Issuer��Audience�����ǲ�������������
                    //Token�䷢����
                    ValidateIssuer = true,
                    //�Ƿ���֤Issuer Token������
                    ValidIssuer = jwtTokenConfig.Issuer,
                    //Token�䷢��˭
                    ValidateAudience = true,
                    //�Ƿ���֤Audience oken������
                    ValidAudience = jwtTokenConfig.Audience,
                    //��֤��Կ�Ƿ���Ч
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.IssuerSigningKey)),
                    // �Ƿ�Ҫ��Token��Claims�б������Expires
                    RequireExpirationTime = true,
                    // ����ķ�����ʱ��ƫ����,��token����ʱ����֤������ʱ��
                    ClockSkew = TimeSpan.FromMinutes(300),
                    // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                    ValidateLifetime = true
                };
            });
            #endregion

            #region ����
            //���cors ���� ���ÿ���������
            services.AddCors(options => options.AddPolicy("cor",
            builder=>
            {
                builder.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials();
            }));
            #endregion


        }

        /// <summary>
        /// ʹ�ô˷�������HTTP����ܵ���
        /// </summary>
        /// <param name="app">IApplicationBuilder ������������Ӧ������ܵ����࣬ASP.NET Core ����ܵ�����һϵ������ί�У����ε��á�</param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            #region ����HTTP����ܵ�
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();//����м��������Ӧ�ó����ṩ��̬��Դ��
            //�������ͨ���������þ�̬�ļ��м���������Զ���ľ�̬��Դ��
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ProjectStaticFile")),
            //    RequestPath = "/StaticFiles"
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion

            #region ��־����
            //���NLog
            loggerFactory.AddNLog();
            //��������
            NLog.LogManager.LoadConfiguration("NLog.config");
            //�����Զ�����м��
            app.UseLog();

            //�ڶ��ַ���
            //Log4Net��־���á��м����ͬС��
            //loggerFactory.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "log4net.config"));
            #endregion

            #region Swagger ����
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //�����м����������Swagger��ΪJSON�ս��
                app.UseSwagger();
                //�����м�������Swagger-UI��ָ��Swagger JSON�ս��
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TMS.API v1");
                });
            }
            #endregion

            #region ʹ��ע������
            //����Cors����
            app.UseCors("cor");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #endregion

            #region ����м�����
            if (env.IsDevelopment())
            {
                //������Ա�쳣ҳ�м�� (UseDeveloperExceptionPage) ����Ӧ������ʱ���󣺴ӹܵ�����ͬ�����첽Exceptionʵ����������HTML������Ӧ��
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                //�쳣��������м�� (UseExceptionHandler) ���������м�����������쳣��
                app.UseExceptionHandler("/Error");
                //HTTP �ϸ��䰲ȫЭ�� (HSTS) �м�� (UseHsts) ��� Strict-Transport-Security ��ͷ��
                app.UseHsts();
            }

            //HTTPS �ض����м�� (UseHttpsRedirection) �� HTTP �����ض��� HTTPS��
            app.UseHttpsRedirection();
            //��̬�ļ��м��(UseStaticFiles) ���ؾ�̬�ļ������򻯽�һ��������
            app.UseStaticFiles();
            //Cookie �����м�� (UseCookiePolicy) ʹӦ�÷���ŷ��һ�����ݱ������� (GDPR) �涨��
            // app.UseCookiePolicy();

            //����·�������·���м�� (UseRouting)��
            app.UseRouting();
            // app.UseRequestLocalization();
            // app.UseCors();

            //�����֤�м�� (UseAuthentication) ���Զ��û����������֤��Ȼ��Ż������û����ʰ�ȫ��Դ��
            app.UseAuthentication();
            //������Ȩ�û����ʰ�ȫ��Դ����Ȩ�м��
            app.UseAuthorization();
            //�Ự�м�� (UseSession) ������ά���Ự״̬�� ���Ӧ��ʹ�ûỰ״̬������ Cookie �����м��֮��� MVC �м��֮ǰ���ûỰ�м����
            // app.UseSession();

            //���ڽ� Razor Pages �ս����ӵ�����ܵ����ս��·���м�������� MapRazorPages �� UseEndpoints����
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion

            #region �쳣�������� 
            //�ж��Ƿ��ǿ�������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//ʹ���쳣��¼ҳ��
            }
            else
            {
                app.UseExceptionHandler("/VehicleManagementAPI/error"); //�����������£�����ϵͳ����ʱ����ת������ҳ��
            }
            //UseStatusCodePages()֧�ֶ�����չ����������һ����������һ��lambda���ʽ:
            app.UseStatusCodePages(async context => {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(
                    "Status code page,status code:" + context.HttpContext.Response.StatusCode);
            });//ʹ��HTTP�������ҳ
            #endregion
        }

        #region ʹ��AutoFac ����ע��
        //ConfigureContainer��������ֱ��ע��ĵط�

        //ʹ��AutoFac������ConfigureServices֮�����У����

        //�˴���������ConfigureServices�н��е�ע�ᡣ

        //��Ҫ���������������������ɵġ�
        /// <summary>
        /// ConfigureContainer��������ֱ��ע��ĵط�
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //������ֱ����AutoFacע�����Լ��Ķ�������Ҫ
            //����builder.Populate(),�ⷢ����AutofacServiceProviderFactory��
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
