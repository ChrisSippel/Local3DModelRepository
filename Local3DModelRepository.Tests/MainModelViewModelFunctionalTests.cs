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
    public sealed class MainModelViewModelFunctionalTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IStorageModule> _storageModule;
        private readonly Mock<IModelsLoader> _modelsLoader;
        private readonly Mock<IDialogService> _dialogService;
        private readonly Mock<ITagsWindowViewModelFactory> _tagsWindowViewModelFactory;

        public MainModelViewModelFunctionalTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _storageModule = _mockRepository.Create<IStorageModule>();
            _modelsLoader = _mockRepository.Create<IModelsLoader>();
            _dialogService = _mockRepository.Create<IDialogService>();
            _tagsWindowViewModelFactory = _mockRepository.Create<ITagsWindowViewModelFactory>();
        }

        /// <summary>
        /// Ensure that we don't clear the already loaded models, if the user tries to load
        /// a repository they've already added.
        /// </summary>
        [Fact]
        public async Task LoadModelsFromStorage_LoadModelsFromFolder_AlreadyLoadedRepository()
        {
            var directory = @"C:\NotARealDirectory";
            var fileName = @"C:\NotARealFile.stl";

            SetupLoadingModelFromStorage(directory, new[] { fileName }, Array.Empty<string>());

            _dialogService
                .Setup(x => x.HaveUserSelectFolder())
                .Returns(Option.Some(directory));

            var mainWindowViewModel = CreateMainWindowViewModel();

            await mainWindowViewModel.LoadModelRespositoriesFromStorage();
            Assert.Single(mainWindowViewModel.ModelViewModels);
            var expectedViewModel = mainWindowViewModel.ModelViewModels.First();

            mainWindowViewModel.OpenFolderCommand.Execute(null);
            Assert.Single(mainWindowViewModel.ModelViewModels);
            Assert.Same(expectedViewModel, mainWindowViewModel.ModelViewModels.First());

            _mockRepository.VerifyAll();
        }

        /// <summary>
        /// Ensure that if models have already been loaded, but we then load a new repository
        /// from a folder, we overwrite the previously loaded models with the ones from the folder.
        /// </summary>
        [Fact]
        public async Task LoadModelsFromStorage_LoadModelsFromFolder_OnlyModelsFromFolder()
        {
            const string repoDirectory = @"C:\MyRepo";

            const string modelFromStorageFileName = @"C:\NotARealFile1.stl";
            var modelFromStorage = SetupLoadingModelFromStorage(repoDirectory, new[] { modelFromStorageFileName }, Array.Empty<string>()).First();

            const string modelFromFolderFileName = @"C:\NotARealFile2.stl";
            var modelFromFolder = SetupLoadingModelsFromFolder(new[] { modelFromFolderFileName }, Array.Empty<string>()).First();

            var mainWindowViewModel = CreateMainWindowViewModel();
            await mainWindowViewModel.LoadModelRespositoriesFromStorage();
            Assert.Single(mainWindowViewModel.ModelViewModels);
            Assert.Same(modelFromStorage.Object, mainWindowViewModel.ModelViewModels[0].Model);

            mainWindowViewModel.OpenFolderCommand.Execute(null);
            Assert.Single(mainWindowViewModel.ModelViewModels);
            Assert.Same(modelFromFolder.Object, mainWindowViewModel.ModelViewModels[0].Model);

            _mockRepository.VerifyAll();
        }

        /// <summary>
        /// Ensure that we can load models from a folder, save those models to a repository file, and then load
        /// those same models files from the repository file.
        /// </summary>
        [Fact]
        public async Task LoadModelsFromFolder_SaveModelRepositoriesToStorage_LoadModelRespositoriesFromStorage_SameModelsLoaded()
        {
            const string modelFromFolderFileName = @"C:\NotARealFile.stl";
            var modelFromFolder = SetupLoadingModelsFromFolder(new[] { modelFromFolderFileName }, Array.Empty<string>()).First();

            var expectedModelFoundFunc = new Func<IModelRepositoryCollection, bool>((modelCollection) =>
            {
                return
                    modelCollection.ModelRepositories.Any(x =>
                        x.Models.Any(y => y == modelFromFolder.Object));
            });

            _storageModule
                .Setup(x => x.Save(It.Is<IModelRepositoryCollection>(y => expectedModelFoundFunc(y))))
                .Returns(ValueTask.CompletedTask);

            SetupLoadingModelFromStorage(null, new[] { @"C:\NotARealFile2.stl" }, Array.Empty<string>());

            var mainWindowViewModel = CreateMainWindowViewModel();
            Assert.Empty(mainWindowViewModel.ModelViewModels);

            mainWindowViewModel.OpenFolderCommand.Execute(null);
            Assert.Single(mainWindowViewModel.ModelViewModels);
            Assert.Same(modelFromFolder.Object, mainWindowViewModel.ModelViewModels[0].Model);

            await mainWindowViewModel.SaveModelRepositoriesToStorage();

            await mainWindowViewModel.LoadModelRespositoriesFromStorage();
            Assert.Equal(2, mainWindowViewModel.ModelViewModels.Count);
            Assert.Null(mainWindowViewModel.SelectedModel);

            _mockRepository.VerifyAll();
        }

        /// <summary>
        /// Ensure that we can load models from a folder, save those models to a repository file, and then load
        /// those same models files from the repository file.
        /// </summary>
        [Fact]
        public async Task ApplyTagsToModel_SaveModelRepositoriesToStorage_LoadModelRespositoriesFromStorage_SameTagsLoaded()
        {
            const string tagValue = "Hell_World";
            const string modelFromFolderFileName = @"C:\NotARealFile.stl";
            var modelFromFolder = SetupLoadingModelsFromFolder(new[] { modelFromFolderFileName }, new[] { tagValue }).First();

            var expectedTagFoundFunc = new Func<IModelRepositoryCollection, bool>((modelCollection) =>
            {
                return
                    modelCollection.ModelRepositories.Any(x =>
                        x.Models.Any(y => y.Tags.Any(z => z.Value == tagValue)));
            });

            _storageModule
                .Setup(x => x.Save(It.Is<IModelRepositoryCollection>(y => expectedTagFoundFunc(y))))
                .Returns(ValueTask.CompletedTask);

            SetupLoadingModelFromStorage(null, new[] { @"C:\NotARealFile2.stl" }, new[] { tagValue });

            var mainWindowViewModel = CreateMainWindowViewModel();
            Assert.Empty(mainWindowViewModel.ModelViewModels);

            mainWindowViewModel.OpenFolderCommand.Execute(null);
            Assert.Single(mainWindowViewModel.ModelViewModels);
            Assert.Same(modelFromFolder.Object, mainWindowViewModel.ModelViewModels[0].Model);

            await mainWindowViewModel.SaveModelRepositoriesToStorage();

            await mainWindowViewModel.LoadModelRespositoriesFromStorage();
            Assert.Equal(2, mainWindowViewModel.ModelViewModels.SelectMany(x => x.Model.Tags).Count());
            Assert.Null(mainWindowViewModel.SelectedModel);

            _mockRepository.VerifyAll();
        }

        private List<Mock<IModel>> SetupLoadingModelFromStorage(
            string repositoryDirectory,
            string[] modelFileNames,
            string[] tags)
        {
            var tagsList = new List<ITag>(tags.Select(x => new Tag(x)));
            var modelsList = CreateModelsForFileNames(modelFileNames);

            foreach (var model in modelsList)
            {
                model
                    .SetupGet(x => x.Tags)
                    .Returns(tagsList);
            }

            var modelRepository = _mockRepository.Create<IModelRepository>();
            modelRepository
                .SetupGet(x => x.Models)
                .Returns(modelsList.Select(x => x.Object));

            if (!string.IsNullOrWhiteSpace(repositoryDirectory))
            {
                modelRepository
                    .SetupGet(x => x.DirectoryPath)
                    .Returns(repositoryDirectory);
            }

            var modelRepositoryCollection = _mockRepository.Create<IModelRepositoryCollection>();
            modelRepositoryCollection
                .SetupGet(x => x.ModelRepositories)
                .Returns(new List<IModelRepository> { modelRepository.Object });

            _storageModule
                .Setup(x => x.Load())
                .Returns(Option.Some(modelRepositoryCollection.Object));

            return modelsList;
        }

        private List<Mock<IModel>> SetupLoadingModelsFromFolder(string[] modelFileNames, string[] tags)
        {
            var tagsList = new List<ITag>(tags.Select(x => new Tag(x)));
            var modelsList = CreateModelsForFileNames(modelFileNames);
            foreach (var model in modelsList)
            {
                model
                    .SetupGet(x => x.Tags)
                    .Returns(tagsList);
            }

            const string ModelFolder = @"C:\NotARealFolder";
            _dialogService
                .Setup(x => x.HaveUserSelectFolder())
                .Returns(Option.Some(ModelFolder));

            _modelsLoader
                .Setup(x => x.LoadAllModels(ModelFolder))
                .Returns(modelsList.Select(x => x.Object));

            return modelsList;
        }

        private List<Mock<IModel>> CreateModelsForFileNames(string[] modelFileNames)
        {
            var modelsList = new List<Mock<IModel>>();
            foreach (var modelFileName in modelFileNames)
            {
                var model = _mockRepository.Create<IModel>();
                model
                    .SetupGet(x => x.FileName)
                    .Returns(modelFileName);

                modelsList.Add(model);
            }

            return modelsList;
        }

        private MainWindowViewModel CreateMainWindowViewModel()
        {
            return new MainWindowViewModel(
                _storageModule.Object,
               _modelsLoader.Object,
               _dialogService.Object,
               _tagsWindowViewModelFactory.Object);
        }
    }
}
