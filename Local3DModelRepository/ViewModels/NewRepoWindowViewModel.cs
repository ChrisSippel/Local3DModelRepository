using Local3DModelRepository.Controls;
using Local3DModelRepository.Models;
using Local3DModelRepository.UiTools;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Local3DModelRepository.ViewModels
{
    public sealed class NewRepoWindowViewModel : INewRepoWindowViewModel
    {
        private static readonly Dictionary<SupportedRepoTypes, (Type pageType, Type viewModelType)> SupportedRepoTypeToPage =
            new Dictionary<SupportedRepoTypes, (Type pageType, Type viewModelType)>
        {
            { SupportedRepoTypes.Local, (typeof(LocalRepoSelectionPage), typeof(LocalRepositoryViewModel)) },
        };

        private bool _atHomePage;

        public NewRepoWindowViewModel()
        {
            _atHomePage = true;

            CloseWithoutSavingCommand = new RelayCommand<IClosableWindow>(NavigateBackwardsImpl);
            CloseAndSaveCommand = new RelayCommand<IClosableWindow>(CloseWindowAndSave, (closableWindow) => NewRepoCreationWindowViewModel != null && NewRepoCreationWindowViewModel.ModelRepsitory.HasValue);
            SelectRepoTypeCommand = new RelayCommand<SupportedRepoTypes>(SelectRepoTypeImpl);
        }

        public ICommand CloseWithoutSavingCommand { get; }

        public ICommand CloseAndSaveCommand { get; }

        public ICommand SelectRepoTypeCommand { get; }

        public bool CanCreateNewRepo { get; set; } = false;

        public INewRepoCreationWindowViewModel NewRepoCreationWindowViewModel { get; private set; }

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
            NewRepoCreationWindowViewModel.PropertyChanged -= NewRepoCreationWindowViewModel_PropertyChanged;
            window.Close();
        }

        private void SelectRepoTypeImpl(SupportedRepoTypes selectedRepoType)
        {
            _atHomePage = false;

            var viewModel = Activator.CreateInstance(SupportedRepoTypeToPage[selectedRepoType].viewModelType);
            NewRepoCreationWindowViewModel = (INewRepoCreationWindowViewModel)viewModel;
            NewRepoCreationWindowViewModel.PropertyChanged += NewRepoCreationWindowViewModel_PropertyChanged;
            NavigateForward?.Invoke(this, SupportedRepoTypeToPage[selectedRepoType].pageType);
        }

        private void NewRepoCreationWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((RelayCommand<IClosableWindow>)CloseAndSaveCommand).NotifyCanExecuteChanged();
        }
    }
}
