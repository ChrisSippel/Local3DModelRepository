using Local3DModelRepository.DataStorage;
using Local3DModelRepository.UiTools;
using Microsoft.Toolkit.Mvvm.Input;
using Optional;
using Optional.Unsafe;
using System;
using System.Windows.Input;

namespace Local3DModelRepository.Repositories.Creation
{
    public sealed class NewRepoWindowViewModel : INewRepoWindowViewModel
    {
        private bool _atHomePage;
        private Option<INewRepoCreationPageViewModel> _newRepoCreationPageViewModel;

        public NewRepoWindowViewModel()
        {
            _atHomePage = true;
            _newRepoCreationPageViewModel = Option.None<INewRepoCreationPageViewModel>();

            CloseWithoutSavingCommand = new RelayCommand<IClosableWindow>(NavigateBackwardsImpl);
            CloseAndSaveCommand = new RelayCommand<IClosableWindow>(CloseWindowAndSave, (closableWindow) => ModelRepository.HasValue);
            SelectRepoTypeCommand = new RelayCommand<SupportedRepoTypes>(SelectRepoTypeImpl);
        }

        public ICommand CloseWithoutSavingCommand { get; }

        public ICommand CloseAndSaveCommand { get; }

        public ICommand SelectRepoTypeCommand { get; }

        public Option<IModelRepository> ModelRepository => _newRepoCreationPageViewModel.HasValue
            ? _newRepoCreationPageViewModel.ValueOrFailure().ModelRepository
            : Option.None<IModelRepository>();

        public event EventHandler<(Type xamlPage, INewRepoCreationPageViewModel viewModel)> NavigateForward;

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
            if (_newRepoCreationPageViewModel.HasValue)
            {
                _newRepoCreationPageViewModel.ValueOrFailure().PropertyChanged -= NewRepoCreationWindowViewModel_PropertyChanged;
            }
            
            window.Close();
        }

        private void SelectRepoTypeImpl(SupportedRepoTypes selectedRepoType)
        {
            _atHomePage = false;

            var viewModel = Activator.CreateInstance(SupportedRepos.SupportedReposByType[selectedRepoType].ViewModel);
            _newRepoCreationPageViewModel = Option.Some((INewRepoCreationPageViewModel)viewModel);
            _newRepoCreationPageViewModel.ValueOrFailure().PropertyChanged += NewRepoCreationWindowViewModel_PropertyChanged;
            NavigateForward?.Invoke(this, (SupportedRepos.SupportedReposByType[selectedRepoType].XamlPage, (INewRepoCreationPageViewModel)viewModel));
        }

        private void NewRepoCreationWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((RelayCommand<IClosableWindow>)CloseAndSaveCommand).NotifyCanExecuteChanged();
        }
    }
}
