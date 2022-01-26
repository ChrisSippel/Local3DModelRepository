using System.IO;

namespace Local3DModelRepository.Wrappers
{
    internal sealed class DirectoryWrapper : IDirectoryWrapper
    {
        public string[] GetFiles(string parentDirectory, string searchPattern, SearchOption searchOption)
            => Directory.GetFiles(parentDirectory, "*.stl", SearchOption.AllDirectories);
    }
}