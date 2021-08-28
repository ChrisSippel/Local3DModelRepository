using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage
{
    public interface IModelRepositoryCollection
    {
        public IEnumerable<ITag> Tags { get; }

        public IEnumerable<IModelRepository> ModelRepositories { get; }
    }
}
