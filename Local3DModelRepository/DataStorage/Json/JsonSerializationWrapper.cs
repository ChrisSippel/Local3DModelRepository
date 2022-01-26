using System.Collections.Generic;
using System.IO;
using Local3DModelRepository.Models;
using Newtonsoft.Json;

namespace Local3DModelRepository.DataStorage.Json
{
    public sealed class JsonSerializationWrapper : IJsonSeralizerWrapper
    {
        private readonly IModelFactory _modelFactory;
        private readonly ITagFactory _tagFactory;
        private readonly IModelRepositoryCollectionFactory _modelRepositoryCollectionFactory;

        public JsonSerializationWrapper(
            IModelFactory modelFactory,
            ITagFactory tagFactory,
            IModelRepositoryCollectionFactory modelRepositoryCollectionFactory)
        {
            _modelFactory = modelFactory;
            _tagFactory = tagFactory;
            _modelRepositoryCollectionFactory = modelRepositoryCollectionFactory;
        }

        public T DeserializeFromFile<T>(string filePath)
        {
            var modelRepositoryJsonConverter = new ModelRepositoryCollectionJsonConverter(
                _modelFactory,
                _tagFactory,
                _modelRepositoryCollectionFactory);

            var convertersList = new List<JsonConverter> { modelRepositoryJsonConverter };
            var jsonSettings = new JsonSerializerSettings()
            {
                Converters = convertersList,
            };

            var serializer = JsonSerializer.Create(jsonSettings);
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);
            var jsonTextReader = new JsonTextReader(streamReader);
            return serializer.Deserialize<T>(jsonTextReader);
        }

        public string Serialize(object? objectToSerialze)
        {
            return JsonConvert.SerializeObject(objectToSerialze);
        }
    }
}