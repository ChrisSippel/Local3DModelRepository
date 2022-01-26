using System;
using System.Collections.Generic;
using System.Linq;
using Local3DModelRepository.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Local3DModelRepository.DataStorage.Json
{
    public sealed class ModelRepositoryCollectionJsonConverter : JsonConverter
    {
        public override bool CanRead { get; } = true;

        public override bool CanWrite { get; } = false;

        private readonly IModelFactory _modelFactory;
        private readonly ITagFactory _tagFactory;
        private readonly IModelRepositoryCollectionFactory _modelRepositoryCollectionFactory;

        public ModelRepositoryCollectionJsonConverter(
            IModelFactory modelFactory,
            ITagFactory tagFactory,
            IModelRepositoryCollectionFactory modelRepositoryCollectionFactory)
        {
            _modelFactory = modelFactory;
            _tagFactory = tagFactory;
            _modelRepositoryCollectionFactory = modelRepositoryCollectionFactory;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IModelRepositoryCollection);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var tags = new List<ITag>();
            var modelRepositories = new List<IModelRepository>();
            while (reader.Read())
            {
                switch (reader.Path)
                {
                    case "Tags":
                        var token = JToken.Load(reader);
                        tags.AddRange(ReadTagsFromToken(token));
                        break;

                    case "ModelRepositories":
                        modelRepositories.AddRange(ReadModelRepositories(reader));
                        break;
                }
            }

            return _modelRepositoryCollectionFactory.Create(tags, modelRepositories);
        }

        private List<IModelRepository> ReadModelRepositories(JsonReader reader)
        {
            var topLevelModelRepositoriesToken = JToken.Load(reader);
            var allModelRepositoriesTokens = topLevelModelRepositoriesToken.Children();

            var modelRepositories = new List<IModelRepository>();
            foreach (var modelRepositoryToken in allModelRepositoriesTokens)
            {
                var directory = string.Empty;
                var models = new List<IModel>();

                var modelRepositoriesValues = modelRepositoryToken.Values();
                foreach (var modelRepositoryValue in modelRepositoriesValues)
                {
                    if (modelRepositoryValue.Path.Contains("DirectoryPath"))
                    {
                        directory = modelRepositoryValue.Value<string>();
                        continue;
                    }

                    if (modelRepositoryValue.Path.Contains("Models"))
                    {
                        models.AddRange(ReadModels(modelRepositoryValue));
                        continue;
                    }
                }
            }

            return modelRepositories;
        }

        private List<IModel> ReadModels(JToken modelsArrayToken)
        {
            var models = new List<IModel>();
            var children = modelsArrayToken.Children();
            foreach (var child in children)
            {
                var fullPath = child.SelectToken("FullPath").Value<string>();
                var fileName = child.SelectToken("FileName").Value<string>();
                var tagsToken = child.SelectToken("Tags");
                var tags = ReadTagsFromToken(tagsToken);

                models.Add( _modelFactory.Create(fullPath));
            }

            return models;
        }

        private List<ITag> ReadTagsFromToken(JToken token)
        {
            var values = token.Values();
            var tagsStrings = values.Values<string>();
            var tagObjectsList = tagsStrings.Select(x => _tagFactory.Create(x));
            return new List<ITag>(tagObjectsList);
        }
    }
}