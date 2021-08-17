using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.API
{
    /// <summary>
    /// Program类
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 主函数（主线程）
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            IHostBuilder hostBuilder = CreateHostBuilder(args);//产生一个IHostBuilder实例hostBuilder ，创建通用主机
            IHost host = hostBuilder.Build();//Build()运行给定操作以初始化主机。 这只能调用一次
            host.Run();//运行应用程序并阻止调用线程，直至主机关闭。 
            //CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// IHostBuilder CreateHostBuilder
        /// </summary> 
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

            //Log4Net日志配置
            .ConfigureLogging(loggingBuilder=>
            {
                //这种注册方式有问题，采用下面的方式
                //loggingBuilder.AddLog4Net("log4net.config");
                //一定要注意文件路径
                loggingBuilder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "log4net.config"));
            })

            .UseServiceProviderFactory(new AutofacServiceProviderFactory())//依赖注入

                .ConfigureWebHostDefaults(webBuilder => 
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureAppConfiguration((h,c)=>//指定引用的配置提供程序
                {
                    c.AddJsonFile($"PersonConfig.Development.json", true, true);
                });
    }
}
