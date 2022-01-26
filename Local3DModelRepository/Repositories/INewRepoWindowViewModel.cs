using System;
using System.Windows.Input;
using Local3DModelRepository.Models;
using Local3DModelRepository.Repositories.Api;
using Local3DModelRepository.UiTools;
using Optional;

namespace Local3DModelRepository.Repositories
{
    public interface INewRepoWindowViewModel : ICloseableSaveableWindow
    {
        ICommand SelectRepoTypeCommand { get; }

        Option<IModelRepository> ModelRepository { get; }

        event EventHandler<(Type xamlPage, INewRepoCreationPageViewModel viewModel)> NavigateForward;

        event EventHandler NavigateBackwards;
    }
}