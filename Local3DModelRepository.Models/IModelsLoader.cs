using System.Collections.Generic;

namespace Local3DModelRepository.Models
{
    public interface IModelsLoader
    {
        IEnumerable<IModel> LoadAllModels(string directory);
    }
}