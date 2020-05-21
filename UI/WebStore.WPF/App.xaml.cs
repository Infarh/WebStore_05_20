﻿using System;
using System.Windows;
using Microsoft.Extensions.Hosting;

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
    }
}
