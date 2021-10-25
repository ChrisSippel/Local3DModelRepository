using System.Windows;
using Local3DModelRepository.DataLoaders;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.DataStorage.Json;
using Local3DModelRepository.FileSystemAccess;
using Local3DModelRepository.Models;
using Local3DModelRepository.UiTools;
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
            var fileWrapper = new FileWrapper();
            var jsonSerializerWrapper = new JsonSerializationWrapper();

            var jsonStoragerModule = new JsonStorageModule("Storage.json", fileWrapper, jsonSerializerWrapper);

            serviceCollection.AddSingleton<IDirectoryWrapper, DirectoryWrapper>();
            serviceCollection.AddSingleton<IJsonSeralizerWrapper>(jsonSerializerWrapper);
            serviceCollection.AddSingleton<IFileWrapper>(fileWrapper);
            serviceCollection.AddSingleton<IStorageModule>(jsonStoragerModule);
            serviceCollection.AddSingleton<IModelsLoader, ModelsLoader>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddSingleton<IModelFactory, ModelFactory>();
            serviceCollection.AddSingleton<IDialogService, DialogService>();
            serviceCollection.AddSingleton<ITagsWindowViewModelFactory, TagsWindowViewModelFactory>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
