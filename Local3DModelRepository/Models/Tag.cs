using System;

namespace Local3DModelRepository.Models
{
    public sealed class Tag : ITag, IEquatable<Tag>
    {
        public Tag(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public bool Equals(Tag other)
        {
            return other != null &&
                other.Value == Value;
        }

        public override int GetHashCode()
        {
            return (Value).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tag);
        }
    }
}
