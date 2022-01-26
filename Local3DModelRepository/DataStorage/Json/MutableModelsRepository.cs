using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage.Json
{
    public sealed class MutableModelsRepository
    {
        public string DirectoryPath { get; set; }

        public IEnumerable<ITag> Tags { get; set; }

        public IEnumerable<IModel> Models { get; set; }
    }
}