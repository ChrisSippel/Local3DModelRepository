using Local3DModelRepository.DataStorage;
using Local3DModelRepository.UiTools;
using Optional;
using System;
using System.Windows.Input;

namespace Local3DModelRepository.Repositories.Creation
{
    public interface INewRepoWindowViewModel : ICloseableSaveableWindow
    {
        ICommand SelectRepoTypeCommand { get; }

        Option<IModelRepository> ModelRepository { get; }

        event EventHandler<(Type xamlPage, INewRepoCreationPageViewModel viewModel)> NavigateForward;

        event EventHandler NavigateBackwards;
    }
}
