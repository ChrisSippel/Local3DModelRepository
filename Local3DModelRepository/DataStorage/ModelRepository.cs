using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage
{
    public sealed class ModelRepository : IModelRepository
    {
        public ModelRepository(
            string directoryPath,
            IEnumerable<IModel> models)
        {
            DirectoryPath = directoryPath;
            Models = models;
        }

        public string DirectoryPath { get; }

        public IEnumerable<IModel> Models { get; }
    }
}
