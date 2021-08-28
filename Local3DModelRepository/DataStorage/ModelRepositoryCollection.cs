using System;
using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage
{
    public sealed class ModelRepositoryCollection : IModelRepositoryCollection
    {
        public IEnumerable<ITag> Tags { get; }

        public IEnumerable<IModelRepository> ModelRepositories { get; }
    }
}
