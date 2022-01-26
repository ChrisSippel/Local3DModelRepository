using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Local3DModelRepository.Models;
using Local3DModelRepository.Wrappers;
using Newtonsoft.Json;
using Optional;

namespace Local3DModelRepository.DataStorage.Json
{
    public sealed class JsonStorageModule : IStorageModule
    {
        private readonly string _filePath;
        private readonly IFileWrapper _fileWrapper;
        private readonly IJsonSeralizerWrapper _jsonSeralizerWrapper;

        public JsonStorageModule(
            string filePath,
            IFileWrapper fileWrapper,
            IJsonSeralizerWrapper jsonSeralizerWrapper)
        {
            _filePath = filePath;
            _fileWrapper = fileWrapper;
            _jsonSeralizerWrapper = jsonSeralizerWrapper;
        }

        public Option<IModelRepositoryCollection> Load()
        {
            if (!_fileWrapper.Exists(_filePath))
            {
                return Option.None<IModelRepositoryCollection>();
            }

            try
            {
                var modelRepositoryCollection =
                    _jsonSeralizerWrapper.DeserializeFromFile<IModelRepositoryCollection>(_filePath);
                return Option.Some(modelRepositoryCollection);
            }
            catch (JsonSerializationException)
            {
                return Option.None<IModelRepositoryCollection>();
            }
        }

        public async ValueTask Save(IModelRepositoryCollection modelRepositoryCollection)
        {
            using var storageFileStream = _fileWrapper.Create(_filePath);

            var modelRepoCollectionAsString = _jsonSeralizerWrapper.Serialize(modelRepositoryCollection);
            var modelRepoCollectionAsByteArray = Encoding.UTF8.GetBytes(modelRepoCollectionAsString);

            await storageFileStream.WriteAsync(modelRepoCollectionAsByteArray, 0, modelRepoCollectionAsByteArray.Length, CancellationToken.None);
        }
    }
}