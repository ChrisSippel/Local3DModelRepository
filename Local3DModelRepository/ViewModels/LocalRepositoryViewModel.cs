using Local3DModelRepository.DataLoaders;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.FileSystemAccess;
using Local3DModelRepository.Models;
using Local3DModelRepository.UiTools;
using Microsoft.Toolkit.Mvvm.Input;
using Optional;
using Optional.Unsafe;
using System.ComponentModel;
using System.Windows.Input;

namespace Local3DModelRepository.ViewModels
{
    public sealed class LocalRepositoryViewModel : INewRepoCreationWindowViewModel
    {
        private Option<IModelRepository> _repository;
        private string _repositoryPath;

        public LocalRepositoryViewModel()
        {
            _repository = Option.None<IModelRepository>();
            _repositoryPath = string.Empty;

            SelectRepoLocationCommand = new RelayCommand(SelectRepositoryLocation);
        }

        public Option<IModelRepository> ModelRepsitory
        {
            get => _repository;
            set
            {
                _repository = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ModelRepsitory)));
            }
        }

        public string RepositoryPath
        {
            get => _repositoryPath;
            set
            {
                _repositoryPath = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(RepositoryPath)));
            }
        }

        public ICommand SelectRepoLocationCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SelectRepositoryLocation()
        {
            var dialogService = new DialogService();
            var userSelectedFolder = dialogService.HaveUserSelectFolder();
            if (!userSelectedFolder.HasValue)
            {
                return;
            }

            RepositoryPath = userSelectedFolder.ValueOrFailure();

            var modelsLoader = new ModelsLoader(new ModelFactory(), new DirectoryWrapper());
            var userSelectedFolderString = userSelectedFolder.ValueOrFailure();
            var loadedModels = modelsLoader.LoadAllModels(userSelectedFolderString);
            ModelRepsitory = Option.Some<IModelRepository>(new LocalModelRepository("Name", userSelectedFolderString, loadedModels));
        }
    }
}
