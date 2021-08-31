namespace Local3DModelRepository.FileSystemAccess
{
    public interface IFileWrapper
    {
        bool Exists(string filePath);

        IStreamWrapper Create(string path);
    }
}
