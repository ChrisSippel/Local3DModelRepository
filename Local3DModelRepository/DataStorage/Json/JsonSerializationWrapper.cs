using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Local3DModelRepository.DataStorage.Json
{
    public sealed class JsonSerializationWrapper : IJsonSeralizerWrapper
    {
        public T DeserializeFromFile<T>(string filePath)
        {
            var modelRepositoryJsonConverter = new ModelRepositoryCollectionJsonConverter();
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
