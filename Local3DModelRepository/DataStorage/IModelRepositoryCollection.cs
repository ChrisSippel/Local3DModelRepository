using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage
{
    public interface IModelRepositoryCollection
    {
        public List<ITag> Tags { get; }

        public List<IModelRepository> ModelRepositories { get; }
    }
}
