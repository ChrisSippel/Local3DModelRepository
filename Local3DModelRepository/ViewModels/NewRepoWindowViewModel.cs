using Local3DModelRepository.Controls;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.Models;
using Local3DModelRepository.UiTools;
using Microsoft.Toolkit.Mvvm.Input;
using Optional;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Local3DModelRepository.ViewModels
{
    public sealed class NewRepoWindowViewModel : INewRepoWindowViewModel
    {
        private static readonly Dictionary<SupportedRepoTypes, Type> SupportedRepoTypeToPage =
            new Dictionary<SupportedRepoTypes, Type>
        {
            { SupportedRepoTypes.Local, typeof(LocalRepoSelectionPage) }
        };

        private bool _atHomePage;

        public NewRepoWindowViewModel()
        {
            _atHomePage = true;

            CloseWithoutSavingCommand = new RelayCommand<IClosableWindow>(NavigateBackwardsImpl);
            CloseAndSaveCommand = new RelayCommand<IClosableWindow>(CloseWindowAndSave, (closableWindow) => CanCreateNewRepo);
            SelectRepoTypeCommand = new RelayCommand<SupportedRepoTypes>(SelectRepoTypeImpl);
        }

        public ICommand CloseWithoutSavingCommand { get; }

        public ICommand CloseAndSaveCommand { get; }

        public ICommand SelectRepoTypeCommand { get; }

        public Option<IModelRepository> ModelRepsitory { get; set; } = Option.None<IModelRepository>();

        public bool CanCreateNewRepo { get; set; } = false;

        public event EventHandler<Type> NavigateForward;

        public event EventHandler NavigateBackwards;

        private void NavigateBackwardsImpl(IClosableWindow window)
        {
            if (_atHomePage)
            {
                window.Close();
            }
            else
            {
                _atHomePage = true;
                NavigateBackwards?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CloseWindowAndSave(IClosableWindow window)
        {
            window.Close();
        }

        private void SelectRepoTypeImpl(SupportedRepoTypes selectedRepoType)
        {
            _atHomePage = false;

            NavigateForward?.Invoke(this, SupportedRepoTypeToPage[selectedRepoType]);
        }
    }
}
