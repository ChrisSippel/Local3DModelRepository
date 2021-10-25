using Local3DModelRepository.Models;
using System.Collections.Generic;

namespace Local3DModelRepository.ViewModels
{
    public sealed class TagsWindowViewModelFactory : ITagsWindowViewModelFactory
    {
        /// <inheritdoc />
        public ITagsWindowViewModel Create(IEnumerable<ITag> possibleTags, IEnumerable<ITag> modelTags)
        {
            return new TagsWindowViewModel(possibleTags, modelTags);
        }
    }
}
