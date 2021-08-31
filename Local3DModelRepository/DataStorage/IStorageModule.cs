using Optional;
using System.Threading.Tasks;

namespace Local3DModelRepository.DataStorage
{
    public interface IStorageModule
    {
        Option<IModelRepositoryCollection> Load();

        ValueTask Save(IModelRepositoryCollection modelRepositoryCollection);
    }
}
