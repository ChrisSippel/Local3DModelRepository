using System.IO;

namespace Local3DModelRepository.FileSystemAccess
{
    public interface IDirectoryWrapper
    {
        string[] GetFiles(string parentDirectory, string searchPattern, SearchOption searchOption);
    }
}
