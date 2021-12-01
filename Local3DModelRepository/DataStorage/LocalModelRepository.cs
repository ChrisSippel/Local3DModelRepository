using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage
{
    public sealed class LocalModelRepository : IModelRepository
    {
        public LocalModelRepository(
            string name,
            string directoryPath,
            IEnumerable<IModel> models)
        {
            Name = name;
            DirectoryPath = directoryPath;
            Models = models;
        }

        public string Name { get; }

        public string DirectoryPath { get; }

        public IEnumerable<IModel> Models { get; }
    }
}
