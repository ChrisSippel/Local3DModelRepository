using Local3DModelRepository.Models;
using Microsoft.Toolkit.Mvvm.Input;
using Optional;
using System.ComponentModel;
using System.Windows.Input;
using Local3DModelRepository.Repositories.Api;
using Local3DModelRepository.Wrappers;
using Optional.Unsafe;

namespace Local3DModelRepository.Repositories.Local
{
    internal sealed class LocalRepoPageViewModel : INewRepoCreationPageViewModel
    {
        private readonly IFolderSelectionDialogWrapper _folderSelectionDialogWrapper;
        private readonly IDirectoryWrapper _directoryWrapper;

        private Option<IModelRepository> _repository;
        private string _repositoryPath;

        public LocalRepoPageViewModel(
            IFolderSelectionDialogWrapper folderSelectionDialogWrapper,
            IDirectoryWrapper directoryWrapper)
        {
            _folderSelectionDialogWrapper = folderSelectionDialogWrapper;
            _directoryWrapper = directoryWrapper;

            _repository = Option.None<IModelRepository>();

            SelectRepoLocationCommand = new RelayCommand(SelectRepo);
        }

        public ICommand SelectRepoLocationCommand { get; }

        public string RepositoryPath
        {
            get => _repositoryPath;
            set
            {
                _repositoryPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RepositoryPath)));
            }
        }

        public Option<IModelRepository> ModelRepository
        {
            get => _repository;
            set
            {
                _repository = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModelRepository)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SelectRepo()
        {
            var userSelectedFolder = _folderSelectionDialogWrapper.HaveUserSelectFolder();
            if (!userSelectedFolder.HasValue)
            {
                return;
            }

            RepositoryPath = userSelectedFolder.ValueOrFailure();

            var userSelectedFolderString = userSelectedFolder.ValueOrFailure();
            ModelRepository = Option.Some<IModelRepository>(new LocalModelRepository("Name", userSelectedFolderString));
        }
    }
}