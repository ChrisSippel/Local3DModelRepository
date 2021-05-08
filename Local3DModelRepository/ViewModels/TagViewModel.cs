using Local3DModelRepository.Models;

namespace Local3DModelRepository.ViewModels
{
    public sealed class TagViewModel
    {
        public TagViewModel(Tag tag)
        {
            Tag = tag;
            DisplayText = tag.Value;
        }

        public Tag Tag { get; }

        public string DisplayText { get; }
    }
}
