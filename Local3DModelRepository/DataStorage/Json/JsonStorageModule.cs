using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Local3DModelRepository.Models;
using Newtonsoft.Json;

namespace Local3DModelRepository.DataStorage.Json
{
    public sealed class JsonStorageModule : IStorageModule
    {
        private readonly string _filePath;
        private readonly HashSet<IModel> _loadedModels;

        public JsonStorageModule(string filePath)
        {
            _filePath = filePath;
            _loadedModels = new HashSet<IModel>();

            LoadedModels = _loadedModels;
        }

        public IReadOnlyCollection<IModel> LoadedModels { get; }

        public void Initialize()
        {
            if (!File.Exists(_filePath))
            {
                return;
            }

            var serializer = new JsonSerializer();
            using var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);
            var jsonTextReader = new JsonTextReader(streamReader);
            var modelsCollection = serializer.Deserialize<ModelsCollection>(jsonTextReader);
            _loadedModels.UnionWith(modelsCollection.Models);
        }

        public void LoadAllModels(string folderPath)
        {
            var allStlFiles = Directory.GetFiles(folderPath, "*.stl", SearchOption.AllDirectories);

            var modelsList = new List<Model>(allStlFiles.Length);
            Array.ForEach(allStlFiles, (string filePath) => { modelsList.Add(new Model(filePath)); });

            var modelsCollection = new ModelsCollection();
            modelsCollection.Models = modelsList;

            using var fileStream = File.Create(_filePath);
            using var streamWriter = new StreamWriter(fileStream);
            var serializer = new JsonSerializer();
            serializer.Serialize(streamWriter, modelsCollection);

            _loadedModels.UnionWith(modelsList);
        }

        public void SaveAllModels()
        {
            var modelsCollection = new ModelsCollection();
            modelsCollection.Models = LoadedModels.Select(x => x as Model);

            using var fileStream = File.OpenWrite(_filePath);
            using var streamWriter = new StreamWriter(fileStream);
            var serializer = new JsonSerializer();
            serializer.Serialize(streamWriter, modelsCollection);
        }
    }
}
