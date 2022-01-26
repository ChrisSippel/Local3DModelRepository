using Local3DModelRepository.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Local3DModelRepository.Wrappers
{
    public sealed class ServiceLoader : IServiceLoader
    {
        public void LoadServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDirectoryWrapper, DirectoryWrapper>();
            serviceCollection.AddSingleton<IFileWrapper, FileWrapper>();
            serviceCollection.AddSingleton<IFolderSelectionDialogWrapper, FolderSelectionDialogWrapper>();
        }
    }
}