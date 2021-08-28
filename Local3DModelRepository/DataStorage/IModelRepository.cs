using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage
{
    public interface IModelRepository
    {
        public string DirectoryPath { get; }

        public IEnumerable<IModel> Models { get; }
    }
}
