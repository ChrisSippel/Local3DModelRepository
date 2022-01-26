namespace Local3DModelRepository.Wrappers
{
    public interface IFileWrapper
    {
        bool Exists(string filePath);

        IStreamWrapper Create(string path);
    }
}