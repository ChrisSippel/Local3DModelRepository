﻿using Local3DModelRepository.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace Local3DModelRepository.ViewModels
{
    public sealed class TagFilterViewModel : ObservableObject, IEquatable<TagFilterViewModel>
    {
        private bool _isChecked;

        public TagFilterViewModel(Tag tag)
            : this(tag, false)
        {
        }

        public TagFilterViewModel(Tag tag, bool isChecked)
        {
            _isChecked = isChecked;

            Tag = tag;
            Value = tag.Value;
        }

        public Tag Tag { get; }

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
            return HashCode.Combine(IsChecked, Value, Tag);
        }
    }
}
