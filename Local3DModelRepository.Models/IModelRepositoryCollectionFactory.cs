using System.Collections.Generic;

namespace Local3DModelRepository.Models
{
    public interface IModelRepositoryCollectionFactory
    {
        IModelRepositoryCollection Create(
            List<ITag> tags,
            List<IModelRepository> modelRepositories);
    }
}