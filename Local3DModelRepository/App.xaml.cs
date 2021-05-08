using System.Windows;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.DataStorage.Json;
using Local3DModelRepository.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Local3DModelRepository
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            AddFeaturesToServiceCollection(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void AddFeaturesToServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IStorageModule>(new JsonStorageModule("Storage.json"));
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
