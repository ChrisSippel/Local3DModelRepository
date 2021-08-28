using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Local3DModelRepository.DataLoaders;
using Local3DModelRepository.FileSystemAccess;
using Local3DModelRepository.Models;
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

        public void Save(IModelRepositoryCollection modelRepositoryCollection)
        {
            /*
            var modelsCollection = new MutableModelsRepository();
            ////modelsCollection.Models = Models.Select(x => x as Model);

            using var fileStream = File.OpenWrite(_filePath);
            using var streamWriter = new StreamWriter(fileStream);
            var serializer = new JsonSerializer();
            serializer.Serialize(streamWriter, modelsCollection);
            */
        }

        public void Save(string repositoryName, IEnumerable<IModel> models)
        {
            /*
            var modelsRepository = new ModelRepository(repositoryName, Enumerable.Empty<ITag>(), models);
            using var fileStream = File.OpenWrite(_filePath);
            using var streamWriter = new StreamWriter(fileStream);
            var serializer = new JsonSerializer();
            serializer.Serialize(streamWriter, modelsRepository);
            */
        }

        private IModelRepository LoadModelsCollectionFromFile()
        {
            /*
            var serializerSettings = new JsonSerializerSettings
            {
                Converters = { new ModelRepositoryJsonConverter() },
            };

            var serializer = JsonSerializer.Create(serializerSettings);
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);
            var jsonTextReader = new JsonTextReader(streamReader);
            return serializer.Deserialize<ModelRepository>(jsonTextReader);
            */
            return null;
        }
    }
}
