using System.Collections.Generic;

namespace Local3DModelRepository.Models
{
    internal sealed class ModelRepositoryCollection : IModelRepositoryCollection
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