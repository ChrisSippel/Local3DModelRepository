using System.IO;

namespace Local3DModelRepository.Wrappers
{
    public interface IDirectoryWrapper
    {
        string[] GetFiles(string parentDirectory, string searchPattern, SearchOption searchOption);
    }
}