using System.IO;

namespace Local3DModelRepository.FileSystemAccess
{
    public sealed class FileWrapper : IFileWrapper
    {
        public bool Exists(string filePath) => File.Exists(filePath);

        public IStreamWrapper Create(string filePath) => new StreamWrapper(File.Create(filePath));
    }
}
