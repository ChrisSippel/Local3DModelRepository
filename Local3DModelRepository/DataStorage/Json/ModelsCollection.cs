using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.DataStorage.Json
{
    public sealed class ModelsCollection
    {
        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<Model> Models { get; set; }
    }
}
