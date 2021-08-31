using Local3DModelRepository.DataStorage;
using Local3DModelRepository.DataStorage.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Local3DModelRepository.Tests
{
    public sealed class ModelRepositoryCollectionJsonConverterUnitTests
    {
        [Fact]
        public void ReadJson_ValidFile()
        {
            var converter = new ModelRepositoryCollectionJsonConverter();
            var textReader = new StreamReader(@"C:\Users\chris\source\repos\Local3DModelRepository\Local3DModelRepository\bin\Debug\net5.0-windows\Storage.json");
            using var reader = new JsonTextReader(textReader);

            var result = (IModelRepositoryCollection)converter.ReadJson(reader, typeof(IModelRepositoryCollection), null, null);
            Assert.NotEmpty(result.ModelRepositories);
        }
    }
}
