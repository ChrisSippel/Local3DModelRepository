using System.IO;
using Local3DModelRepository.Models;
using Xunit;

namespace Local3DModelRepository.Tests
{
    public sealed class ModelFactoryUnitTests
    {
        [Fact]
        public void Create_ValidModel()
        {
            var filePath = @"C:\users\bob\MyFile.stl";

            var modelFactory = new ModelFactory();
            var model = modelFactory.Create(filePath);

            Assert.Equal(filePath, model.FullPath);
            Assert.Equal(Path.GetFileName(filePath), model.FileName);
            Assert.Empty(model.Tags);
        }
    }
}
