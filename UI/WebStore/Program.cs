using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args) => 
            CreateHostBuilder(args)
               .Build()
               .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((host, log) =>
                {
                    //log.ClearProviders();
                    //log.AddProvider(new ConsoleLoggerProvider())
                    //log.AddConsole(opt => opt.IncludeScopes = true);
                    //log.AddEventLog(opt => opt.LogName = "WebStore");
                    //log.AddFilter("System", LogLevel.Warning);
                    //log.AddFilter((category, level) =>
                    //{
                    //    if (category.StartsWith("Microsoft")) 
                    //        return level >= LogLevel.Warning;
                    //    return true;
                    //});
                })
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                });
    }
}
