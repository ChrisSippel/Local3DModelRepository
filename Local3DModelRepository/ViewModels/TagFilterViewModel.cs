using System;
using Local3DModelRepository.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Local3DModelRepository.ViewModels
{
    public sealed class TagFilterViewModel : ObservableObject, IEquatable<TagFilterViewModel>
    {
        private bool _isChecked;

        public TagFilterViewModel(ITag tag)
            : this(tag, false)
        {
        }

        public TagFilterViewModel(ITag tag, bool isChecked)
        {
            _isChecked = isChecked;

            Tag = tag;
            Value = tag.Value;
        }

        public ITag Tag { get; }

        public string Value { get; }

        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        public bool Equals(TagFilterViewModel other)
        {
            return other != null &&
                   other.Value == Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TagFilterViewModel);
        }

        public override int GetHashCode()
        {
            return (IsChecked, Value, Tag).GetHashCode();
        }
    }
}