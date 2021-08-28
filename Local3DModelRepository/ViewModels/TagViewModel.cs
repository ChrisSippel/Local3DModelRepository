using Local3DModelRepository.Models;

namespace Local3DModelRepository.ViewModels
{
    public sealed class TagViewModel
    {
        public TagViewModel(ITag tag)
        {
            Tag = tag;
            DisplayText = tag.Value;
        }

        public ITag Tag { get; }

        public string DisplayText { get; }
    }
}
