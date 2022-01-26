using System.Threading.Tasks;
using Local3DModelRepository.Models;
using Optional;

namespace Local3DModelRepository.DataStorage
{
    public interface IStorageModule
    {
        Option<IModelRepositoryCollection> Load();

        ValueTask Save(IModelRepositoryCollection modelRepositoryCollection);
    }
}