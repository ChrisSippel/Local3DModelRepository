using System.Collections.Generic;
using System.IO;
using System.Linq;
using Local3DModelRepository.FileSystemAccess;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataLoaders
{
    public sealed class ModelsLoader : IModelsLoader
    {
        private readonly IModelFactory _modelFactory;
        private readonly IDirectoryWrapper _directoryWrapper;

        public ModelsLoader(
            IModelFactory modelFactory,
            IDirectoryWrapper directoryWrapper)
        {
            _modelFactory = modelFactory;
            _directoryWrapper = directoryWrapper;
        }

        public IEnumerable<IModel> LoadAllModels(string directory)
        {
            var allStlFiles = _directoryWrapper.GetFiles(directory, "*.stl", SearchOption.AllDirectories);
            return allStlFiles.Select(stlFilePath => _modelFactory.Create(stlFilePath)).ToArray();
        }
    }
}
