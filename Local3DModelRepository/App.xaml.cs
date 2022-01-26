using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Local3DModelRepository.Api;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.DataStorage.Json;
using Local3DModelRepository.Repositories;
using Local3DModelRepository.UiTools;
using Local3DModelRepository.ViewModels;
using Local3DModelRepository.Wrappers;
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
            AddOtherAssemblyFeaturesToServiceCollection(serviceCollection);
            AddLocalFeaturesToServiceCollection(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void AddOtherAssemblyFeaturesToServiceCollection(IServiceCollection serviceCollection)
        {
            var dllsToLookAt = Directory.GetFiles(Directory.GetCurrentDirectory(), "Local3DModelRepository.*.dll", SearchOption.TopDirectoryOnly);
            foreach (var dll in dllsToLookAt)
            {
                var dllAssembly = Assembly.LoadFile(dll);
                var serviceLoaderTypes = dllAssembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IServiceLoader)));
                foreach (var serviceLoaderType in serviceLoaderTypes)
                {
                    var serviceLoader = (IServiceLoader)Activator.CreateInstance(serviceLoaderType);
                    serviceLoader.LoadServices(serviceCollection);

                }
            }
        }

        private void AddLocalFeaturesToServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IStorageModule>(serviceProvider =>
                new JsonStorageModule(
                    "Storage.json",
                    serviceProvider.GetService<IFileWrapper>(),
                    serviceProvider.GetService<JsonSerializationWrapper>()));

            serviceCollection.AddSingleton<ITagsWindowViewModelFactory, TagsWindowViewModelFactory>();
            serviceCollection.AddSingleton<IDialogService, DialogService>();
            serviceCollection.AddSingleton<IJsonSeralizerWrapper, JsonSerializationWrapper>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddTransient<INewRepoTypeSelectionPageViewModel, NewRepoTypeSelectionPageViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}