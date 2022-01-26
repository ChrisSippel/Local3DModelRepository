using Optional;
using System.ComponentModel;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.Repositories.Api
{
    public interface INewRepoCreationPageViewModel : INotifyPropertyChanged
    {
        Option<IModelRepository> ModelRepository { get; }
    }
}