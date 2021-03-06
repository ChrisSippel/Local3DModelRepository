using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.DataStorage.Json;
using Local3DModelRepository.FileSystemAccess;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Local3DModelRepository.Tests
{
    public sealed class JsonStorageModuleUnitTests : IDisposable
    {
        const string FilePath = @"C:\MyFile.stl";

        private readonly MockRepository _mockRepository;

        private readonly Mock<IFileWrapper> _fileWrapper;
        private readonly Mock<IJsonSeralizerWrapper> _jsonSeralizerWrapper;

        public JsonStorageModuleUnitTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _fileWrapper = _mockRepository.Create<IFileWrapper>();
            _jsonSeralizerWrapper = _mockRepository.Create<IJsonSeralizerWrapper>();
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Load_FileDoesExist()
        {
            _fileWrapper
                .Setup(x => x.Exists(FilePath))
                .Returns(false);

            var jsonStorageModule = CreateJsonStorageModule();
            var result = jsonStorageModule.Load();

            Assert.False(result.HasValue);
        }

        [Fact]
        public void Load_FileExists_DeserializationThrowsException()
        {
            _fileWrapper
                .Setup(x => x.Exists(FilePath))
                .Returns(true);

            _jsonSeralizerWrapper
                .Setup(x => x.DeserializeFromFile<IModelRepositoryCollection>(FilePath))
                .Throws<JsonSerializationException>();

            var jsonStorageModule = CreateJsonStorageModule();
            var result = jsonStorageModule.Load();

            Assert.False(result.HasValue);
        }

        [Fact]
        public void Load_FileExists_DeserializationIsSuccessful()
        {
            _fileWrapper
                .Setup(x => x.Exists(FilePath))
                .Returns(true);

            var modelRepositoryCollection = _mockRepository.Create<IModelRepositoryCollection>();
            _jsonSeralizerWrapper
                .Setup(x => x.DeserializeFromFile<IModelRepositoryCollection>(FilePath))
                .Returns(modelRepositoryCollection.Object);

            var jsonStorageModule = CreateJsonStorageModule();
            var result = jsonStorageModule.Load();

            Assert.True(result.HasValue);
        }

        [Fact]
        public async ValueTask Save_SavingIsSuccessful()
        {
            const string SerializedString = "NotARealJsonObject";

            var objectToSave = _mockRepository.Create<IModelRepositoryCollection>();

            var stream = _mockRepository.Create<IStreamWrapper>();
            stream
                .Setup(x => x.WriteAsync(It.IsAny<ReadOnlyMemory<byte>>(), CancellationToken.None))
                .Returns(ValueTask.CompletedTask);

            _fileWrapper
                .Setup(x => x.Create(FilePath))
                .Returns(stream.Object);

            _jsonSeralizerWrapper
                .Setup(x => x.Serialize(objectToSave.Object))
                .Returns(SerializedString);

            var jsonStorageModule = CreateJsonStorageModule();
            await jsonStorageModule.Save(objectToSave.Object);
        }

        private JsonStorageModule CreateJsonStorageModule()
        {
            return new JsonStorageModule(
                FilePath,
                _fileWrapper.Object,
                _jsonSeralizerWrapper.Object);
        }
    }
}
