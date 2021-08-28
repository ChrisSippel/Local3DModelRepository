using System;
using Local3DModelRepository.DataLoaders;
using Local3DModelRepository.FileSystemAccess;
using Local3DModelRepository.Models;
using Xunit;
using Moq;
using System.IO;
using System.Linq;

namespace Local3DModelRepository.Tests
{
    public sealed class ModelsLoaderUnitTests : IDisposable
    {
        private readonly MockRepository _mockRepository;

        public ModelsLoaderUnitTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void LoadAllModels_NoDirectory_NoModels()
        {
            var directory = string.Empty;

            var modelFactory = _mockRepository.Create<IModelFactory>();

            var directoryWrapper = _mockRepository.Create<IDirectoryWrapper>();
            directoryWrapper
                .Setup(x => x.GetFiles(directory, "*.stl", SearchOption.AllDirectories))
                .Returns(Array.Empty<string>());

            var modelsLoader = new ModelsLoader(modelFactory.Object, directoryWrapper.Object);
            var actual = modelsLoader.LoadAllModels(directory);

            Assert.Empty(actual);
        }

        [Fact]
        public void LoadAllModels_SingleFile_SingleModel()
        {
            var directory = "NotARealDirectory";
            var file = "NotARealFile";

            var model = _mockRepository.Create<IModel>();

            var modelFactory = _mockRepository.Create<IModelFactory>();
            modelFactory
                .Setup(x => x.Create(file))
                .Returns(model.Object);

            var directoryWrapper = _mockRepository.Create<IDirectoryWrapper>();
            directoryWrapper
                .Setup(x => x.GetFiles(directory, "*.stl", SearchOption.AllDirectories))
                .Returns(new[] { file });

            var modelsLoader = new ModelsLoader(modelFactory.Object, directoryWrapper.Object);
            var actual = modelsLoader.LoadAllModels(directory);

            Assert.Single(actual);
            Assert.Same(model.Object, actual.First());
        }

        [Fact]
        public void LoadAllModels_MultipleFiles_MultipleModels()
        {
            var directory = "NotARealDirectory";
            var fileOne = "FileOne";
            var fileTwo = "FileTwo";

            var modelOne = _mockRepository.Create<IModel>();
            var modelTwo = _mockRepository.Create<IModel>();

            var modelFactory = _mockRepository.Create<IModelFactory>();
            modelFactory
                .Setup(x => x.Create(fileOne))
                .Returns(modelOne.Object);

            modelFactory
                .Setup(x => x.Create(fileTwo))
                .Returns(modelTwo.Object);

            var directoryWrapper = _mockRepository.Create<IDirectoryWrapper>();
            directoryWrapper
                .Setup(x => x.GetFiles(directory, "*.stl", SearchOption.AllDirectories))
                .Returns(new[] { fileOne, fileTwo });

            var modelsLoader = new ModelsLoader(modelFactory.Object, directoryWrapper.Object);
            var actual = modelsLoader.LoadAllModels(directory);

            Assert.Equal(2, actual.Count());
            Assert.Same(modelOne.Object, actual.First());
            Assert.Same(modelTwo.Object, actual.ElementAt(1));
        }
    }
}
