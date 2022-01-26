using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.ViewModels
{
    public sealed class TagsWindowViewModelFactory : ITagsWindowViewModelFactory
    {
        private readonly ITagFactory _tagFactory;

        public TagsWindowViewModelFactory(ITagFactory tagFactory)
        {
            _tagFactory = tagFactory;
        }

        /// <inheritdoc />
        public ITagsWindowViewModel Create(IEnumerable<ITag> possibleTags, IEnumerable<ITag> modelTags)
        {
            return new TagsWindowViewModel(_tagFactory, possibleTags, modelTags);
        }
    }
}