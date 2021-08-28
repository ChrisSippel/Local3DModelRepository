using System.IO;
using Newtonsoft.Json;

namespace Local3DModelRepository.DataStorage.Json
{
    public sealed class JsonSerializationWrapper : IJsonSeralizerWrapper
    {
        public T DeserializeFromFile<T>(string filePath)
        {
            var serializer = JsonSerializer.Create();
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);
            var jsonTextReader = new JsonTextReader(streamReader);
            return serializer.Deserialize<T>(jsonTextReader);
        }
    }
}
