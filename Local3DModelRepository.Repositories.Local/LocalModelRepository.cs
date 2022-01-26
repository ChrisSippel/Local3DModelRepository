using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.Repositories.Local
{
    public sealed class LocalModelRepository : IModelRepository
    {
        public LocalModelRepository(
            string name,
            string directoryPath)
        {
            Name = name;
            DirectoryPath = directoryPath;
        }

        public string Name { get; }

        public string DirectoryPath { get; }

        public IEnumerable<IModel> Models { get; }
    }
}