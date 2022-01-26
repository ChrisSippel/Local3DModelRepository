using System.Collections.Generic;

namespace Local3DModelRepository.Models
{
    public interface IModelRepositoryCollection
    {
        List<ITag> Tags { get; }

        List<IModelRepository> ModelRepositories { get; }
    }
}