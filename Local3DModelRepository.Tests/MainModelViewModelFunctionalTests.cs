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
    public sealed class MainModelViewModelFunctionalTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IStorageModule> _storageModule;
        private readonly Mock<IModelsLoader> _modelsLoader;
        private readonly Mock<IDialogService> _dialogService;

        public MainModelViewModelFunctionalTests()
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
        public async Task LoadExistingModelRespositories_LoadModelsFromFolder_OnlyModelsFromFolder()
        {
            // Set-ups for loading models from an existing model repository
            var alreadyExistingModel = _mockRepository.Create<IModel>();
            alreadyExistingModel
                .SetupGet(x => x.FileName)
                .Returns(@"C:\NotARealFile1.stl");

            var modelRepository = _mockRepository.Create<IModelRepository>();
            modelRepository
                .SetupGet(x => x.Models)
                .Returns(new[] { alreadyExistingModel.Object });

            var modelRepositoryCollection = _mockRepository.Create<IModelRepositoryCollection>();
            modelRepositoryCollection
                .SetupGet(x => x.ModelRepositories)
                .Returns(new[] { modelRepository.Object });

            _storageModule
                .Setup(x => x.Load())
                .Returns(Option.Some(modelRepositoryCollection.Object));

            // Set-ups for loading models from a folder
            const string ModelFolder = @"C:\NotARealFolder";
            _dialogService
                .Setup(x => x.HaveUserSelectFolder())
                .Returns(Option.Some(ModelFolder));

            var modelFromFolder = _mockRepository.Create<IModel>();
            modelFromFolder
                .SetupGet(x => x.FileName)
                .Returns(@"C:\NotARealFile2.stl");

            _modelsLoader
                .Setup(x => x.LoadAllModels(ModelFolder))
                .Returns(new[] { modelFromFolder.Object });

            var mainWindowViewModel = CreateMainWindowViewModel();
            await mainWindowViewModel.LoadExistingModelRespositories();
            Assert.Single(mainWindowViewModel.ModelViewModels);
            Assert.Same(alreadyExistingModel.Object, mainWindowViewModel.ModelViewModels[0].Model);

            // Load models from folder
            mainWindowViewModel.OpenFolderCommand.Execute(null);
            Assert.Single(mainWindowViewModel.ModelViewModels);
            Assert.Same(modelFromFolder.Object, mainWindowViewModel.ModelViewModels[0].Model);
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
