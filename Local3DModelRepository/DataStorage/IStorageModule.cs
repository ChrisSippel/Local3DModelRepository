using Optional;

namespace Local3DModelRepository.DataStorage
{
    public interface IStorageModule
    {
        Option<IModelRepositoryCollection> Load();

        void Save(IModelRepositoryCollection modelRepositoryCollection);
    }
}
