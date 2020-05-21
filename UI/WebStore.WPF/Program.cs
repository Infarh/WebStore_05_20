using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WebStore.WPF
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args)
               .UseContentRoot(App.CurrentDirectory)
               .ConfigureAppConfiguration((host, cfg) =>
                    cfg.SetBasePath(App.CurrentDirectory)
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
               .ConfigureServices(App.ConfigureServices);
    }
}
