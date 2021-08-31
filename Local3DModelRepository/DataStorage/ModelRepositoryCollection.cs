using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage
{
    public sealed class ModelRepositoryCollection : IModelRepositoryCollection
    {
        public ModelRepositoryCollection()
            : this(new List<ITag>(), new List<IModelRepository>())
        {
        }

        public ModelRepositoryCollection(
            List<ITag> tags,
            List<IModelRepository> modelRepositories)
        {
            Tags = tags;
            ModelRepositories = modelRepositories;
        }

        public List<ITag> Tags { get; }

        public List<IModelRepository> ModelRepositories { get; }
    }
}
