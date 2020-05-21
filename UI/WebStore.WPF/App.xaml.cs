using System.Windows;

namespace WebStore.WPF
{
    public partial class App
    {
        private static bool __IsDesignTime = true;
        public static bool IsDesignTime => __IsDesignTime;

        protected override void OnStartup(StartupEventArgs e)
        {
            __IsDesignTime = false;

            base.OnStartup(e);
        }
    }
}
