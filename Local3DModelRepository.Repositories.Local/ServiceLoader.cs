using System.Diagnostics;
using Local3DModelRepository.Api;
using Local3DModelRepository.Repositories.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Local3DModelRepository.Repositories.Local
{
    public sealed class ServiceLoader : IServiceLoader
    {
        public void LoadServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISupportedRepoInformation, SupportedRepoInformation>(provider => new SupportedRepoInformation(provider.GetService<LocalRepoPageViewModel>()));
            serviceCollection.AddTransient<LocalRepoPageViewModel>();
        }
    }
}