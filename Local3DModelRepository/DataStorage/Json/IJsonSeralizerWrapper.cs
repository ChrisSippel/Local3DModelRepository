namespace Local3DModelRepository.DataStorage.Json
{
    public interface IJsonSeralizerWrapper
    {
        T DeserializeFromFile<T>(string filePath);

        string Serialize(object? objectToSerialze);
    }
}
