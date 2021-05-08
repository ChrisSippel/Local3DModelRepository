using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage
{
    public interface IStorageModule
    {
        IReadOnlyCollection<IModel> LoadedModels { get; }

        void Initialize();

        void LoadAllModels(string folderPath);

        void SaveAllModels();
    }
}
