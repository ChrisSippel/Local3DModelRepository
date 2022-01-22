using Local3DModelRepository.DataStorage;
using Optional;
using System.ComponentModel;

namespace Local3DModelRepository.Repositories.Creation
{
    public interface INewRepoCreationPageViewModel : INotifyPropertyChanged
    {
        Option<IModelRepository> ModelRepository { get; }
    }
}
