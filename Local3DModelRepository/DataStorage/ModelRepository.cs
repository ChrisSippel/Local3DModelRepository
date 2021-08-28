using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage
{
    public sealed class ModelRepository : IModelRepository
    {
        public ModelRepository(
            string directoryPath,
            IEnumerable<ITag> tags,
            IEnumerable<IModel> models)
        {
            DirectoryPath = directoryPath;
            Tags = tags;
            Models = models;
        }

        public string DirectoryPath { get; }

        public IEnumerable<ITag> Tags { get; }

        public IEnumerable<IModel> Models { get; }
    }
}
