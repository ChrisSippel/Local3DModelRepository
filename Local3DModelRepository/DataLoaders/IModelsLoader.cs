using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataLoaders
{
    public interface IModelsLoader
    {
        IEnumerable<IModel> LoadAllModels(string directory);
    }
}
