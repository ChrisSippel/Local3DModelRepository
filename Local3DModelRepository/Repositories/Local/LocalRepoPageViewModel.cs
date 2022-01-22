using Local3DModelRepository.DataLoaders;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.FileSystemAccess;
using Local3DModelRepository.Models;
using Local3DModelRepository.Repositories.Creation;
using Local3DModelRepository.UiTools;
using Microsoft.Toolkit.Mvvm.Input;
using Optional;
using Optional.Unsafe;
using System.ComponentModel;
using System.Windows.Input;

namespace Local3DModelRepository.Repositories.Local
{
    public sealed class LocalRepoPageViewModel : INewRepoCreationPageViewModel, INotifyPropertyChanged
    {
        private Option<IModelRepository> _repository;
        private string _repositoryPath;

        public LocalRepoPageViewModel()
        {
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
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(RepositoryPath)));
            }
        }

        public Option<IModelRepository> ModelRepository
        {
            get => _repository;
            set
            {
                _repository = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ModelRepository)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SelectRepo()
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
            ModelRepository = Option.Some<IModelRepository>(new LocalModelRepository("Name", userSelectedFolderString, loadedModels));
        }
    }
}
