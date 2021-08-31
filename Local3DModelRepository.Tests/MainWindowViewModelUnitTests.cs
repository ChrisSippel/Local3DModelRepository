using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Local3DModelRepository.DataLoaders;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.Models;
using Local3DModelRepository.UiTools;
using Local3DModelRepository.ViewModels;
using Moq;
using Optional;
using Xunit;

namespace Local3DModelRepository.Tests
{
    public sealed class MainWindowViewModelUnitTests : IDisposable
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IStorageModule> _storageModule;
        private readonly Mock<IModelsLoader> _modelsLoader;
        private readonly Mock<IDialogService> _dialogService;

        public MainWindowViewModelUnitTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _storageModule = _mockRepository.Create<IStorageModule>();
            _modelsLoader = _mockRepository.Create<IModelsLoader>();
            _dialogService = _mockRepository.Create<IDialogService>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void OpenFolderCommand_NoFolderSelected()
        {
            _dialogService
                .Setup(x => x.HaveUserSelectFolder())
                .Returns(Option.None<string>());

            var mainModelViewModel = CreateMainWindowViewModel();

            mainModelViewModel.OpenFolderCommand.Execute(null);

            Assert.Empty(mainModelViewModel.ModelViewModels);
        }

        [Fact]
        public void OpenFolderCommand_FolderSelected_NoModelsInside()
        {
            const string SelectedFolder = "MyFolder";

            _dialogService
                .Setup(x => x.HaveUserSelectFolder())
                .Returns(Option.Some(SelectedFolder));

            _modelsLoader
                .Setup(x => x.LoadAllModels(SelectedFolder))
                .Returns(Enumerable.Empty<IModel>);

            var mainModelViewModel = CreateMainWindowViewModel();

            mainModelViewModel.OpenFolderCommand.Execute(null);

            Assert.Empty(mainModelViewModel.ModelViewModels);
        }

        [Fact]
        public void OpenFolderCommand_FolderSelected_ModelsAreInside()
        {
            const string SelectedFolder = "MyFolder";

            _dialogService
                .Setup(x => x.HaveUserSelectFolder())
                .Returns(Option.Some(SelectedFolder));

            var model = _mockRepository.Create<IModel>();
            model
                .SetupGet(x => x.FileName)
                .Returns(@"C:\MyModel.stl");

            _modelsLoader
                .Setup(x => x.LoadAllModels(SelectedFolder))
                .Returns(new[] { model.Object });

            var mainModelViewModel = CreateMainWindowViewModel();

            mainModelViewModel.OpenFolderCommand.Execute(null);

            Assert.Single(mainModelViewModel.ModelViewModels);
            Assert.Same(model.Object, mainModelViewModel.ModelViewModels[0].Model);
        }

        [Fact]
        public void SelectionChangedCommand_NullViewModel()
        {
            var mainModelViewModel = CreateMainWindowViewModel();

            mainModelViewModel.SelectionChangedCommand.Execute(null);
            Assert.Null(mainModelViewModel.SelectedModel);
            Assert.False(mainModelViewModel.IsLoading);
        }

        [Fact]
        public async Task LoadExistingModelRespositories_NoModelRepositoryCollection()
        {
            _storageModule
                .Setup(x => x.Load())
                .Returns(Option.None<IModelRepositoryCollection>());

            var mainWindowViewModel = CreateMainWindowViewModel();
            await mainWindowViewModel.LoadModelRespositoriesFromStorage();
            Assert.Empty(mainWindowViewModel.ModelViewModels);
        }

        [Fact]
        public async Task LoadExistingModelRespositories_ModelRepositoryCollectionExists()
        {
            var model = _mockRepository.Create<IModel>();
            model
                .SetupGet(x => x.FileName)
                .Returns(@"C:\NotARealFile.stl");

            var modelRepository = _mockRepository.Create<IModelRepository>();
            modelRepository
                .SetupGet(x => x.Models)
                .Returns(new[] { model.Object });

            var modelRepositoryCollection = _mockRepository.Create<IModelRepositoryCollection>();
            modelRepositoryCollection
                .SetupGet(x => x.ModelRepositories)
                .Returns(new List<IModelRepository> { modelRepository.Object });

            _storageModule
                .Setup(x => x.Load())
                .Returns(Option.Some(modelRepositoryCollection.Object));

            var mainWindowViewModel = CreateMainWindowViewModel();
            await mainWindowViewModel.LoadModelRespositoriesFromStorage();
            Assert.Single(mainWindowViewModel.ModelViewModels);
            Assert.Same(model.Object, mainWindowViewModel.ModelViewModels[0].Model);
        }

        [Fact]
        public async Task SaveModelRepositories_SaveSuccessfully()
        {
            _storageModule
                .Setup(x => x.Save(It.IsAny<IModelRepositoryCollection>()))
                .Returns(ValueTask.CompletedTask);

            var mainWindowViewModel = CreateMainWindowViewModel();
            await mainWindowViewModel.SaveModelRepositoriesToStorage();

            Assert.False(mainWindowViewModel.IsLoading);
        }

        private MainWindowViewModel CreateMainWindowViewModel()
        {
            return new MainWindowViewModel(
                _storageModule.Object,
               _modelsLoader.Object,
               _dialogService.Object);
        }
    }
}
