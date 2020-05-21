using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Clients.Employees;
using WebStore.Interfaces.Services;
using WebStore.WPF.ViewModels;

namespace WebStore.WPF
{
    public partial class App
    {
        private static bool __IsDesignTime = true;
        public static bool IsDesignTime => __IsDesignTime;

        private static IHost __Host;

        public static IHost Host => __Host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        protected override async void OnStartup(StartupEventArgs e)
        {
            __IsDesignTime = false;

            var host = Host;

            base.OnStartup(e);

            await host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var host = Host;
            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
            __Host = null;
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddTransient(s => new MainWindow { DataContext = s.GetRequiredService<MainWindowViewModel>()});
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<IEmployeesData, EmployeesClient>();
        }

        public static string CurrentDirectory => IsDesignTime ? Path.GetDirectoryName(GetSourceCodePath()) : Environment.CurrentDirectory;

        public static string GetSourceCodePath([CallerFilePath] string path = null) => path;
    }
}
