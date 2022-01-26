using System.Collections.Generic;

namespace Local3DModelRepository.Models
{
    public interface IModelRepository
    {
        string Name { get; }

        string DirectoryPath { get; }

        IEnumerable<IModel> Models { get; }
    }
}