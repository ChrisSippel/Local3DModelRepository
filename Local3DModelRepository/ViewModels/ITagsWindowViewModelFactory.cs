using System.Collections.Generic;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.ViewModels
{
    public interface ITagsWindowViewModelFactory
    {
        /// <summary>
        /// Creates a new <seealso cref="ITagsWindowViewModel"/> object.
        /// </summary>
        /// <param name="possibleTags">The collection of pre-existing tags the user can select.</param>
        /// <param name="modelTags">The tags that have been applied to the model already.</param>
        /// <returns>A new <seealso cref="ITagsWindowViewModel"/> object.</returns>
        ITagsWindowViewModel Create(IEnumerable<ITag> possibleTags, IEnumerable<ITag> modelTags);
    }
}