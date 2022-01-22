using Local3DModelRepository.DataStorage;
using Optional;
using System.ComponentModel;

namespace Local3DModelRepository.ViewModels
{
    public interface INewRepoCreationWindowViewModel : INotifyPropertyChanged
    {
        Option<IModelRepository> ModelRepsitory { get; }
    }
}
