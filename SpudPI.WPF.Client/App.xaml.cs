using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace SpudPI.WPF.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public readonly IServiceProvider ServiceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<MainWindow>(s => new MainWindow(ServiceProvider!.GetRequiredService<INavigationService>())
            {
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }
    }

}
