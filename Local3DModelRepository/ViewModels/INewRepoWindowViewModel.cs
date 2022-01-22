using Local3DModelRepository.UiTools;
using System;
using System.Windows.Input;

namespace Local3DModelRepository.ViewModels
{
    public interface INewRepoWindowViewModel : ICloseableSaveableWindow
    {
        ICommand SelectRepoTypeCommand { get; }

        INewRepoCreationWindowViewModel NewRepoCreationWindowViewModel { get; }

        event EventHandler<Type> NavigateForward;

        event EventHandler NavigateBackwards;
    }
}
