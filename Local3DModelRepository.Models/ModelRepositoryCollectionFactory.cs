using System.Collections.Generic;

namespace Local3DModelRepository.Models
{
    internal sealed class ModelRepositoryCollectionFactory : IModelRepositoryCollectionFactory
    {
        public IModelRepositoryCollection Create(
            List<ITag> tags,
            List<IModelRepository> modelRepositories) =>
                new ModelRepositoryCollection(tags, modelRepositories);
    }
}