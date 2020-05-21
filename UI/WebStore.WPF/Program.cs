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
               .ConfigureAppConfiguration((host, cfg) =>
                    cfg.AddJsonFile("appsettings.json", true, true));
    }
}
