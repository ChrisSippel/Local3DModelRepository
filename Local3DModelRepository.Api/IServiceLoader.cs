using Microsoft.Extensions.DependencyInjection;

namespace Local3DModelRepository.Api
{
    public interface IServiceLoader
    {
        void LoadServices(IServiceCollection serviceCollection);
    }
}