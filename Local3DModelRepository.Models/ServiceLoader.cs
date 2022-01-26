using Local3DModelRepository.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Local3DModelRepository.Models
{
    public sealed class ServiceLoader : IServiceLoader
    {
        public void LoadServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IModelFactory, ModelFactory>();
            serviceCollection.AddSingleton<IModelsLoader, ModelsLoader>();
            serviceCollection.AddSingleton<ITagFactory, TagFactory>();
            serviceCollection.AddSingleton<IModelRepositoryCollectionFactory, ModelRepositoryCollectionFactory>();
        }
    }
}