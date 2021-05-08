using System;
using System.Collections.Generic;
using System.IO;

namespace Local3DModelRepository.Models
{
    public sealed class Model : IModel, IEquatable<IModel>
    {
        private readonly List<Tag> _tags;

        public Model(string fullPath)
        {
            _tags = new List<Tag>();

            FullPath = fullPath;
            FileName = Path.GetFileName(fullPath);
            Tags = _tags;
        }

        public string FullPath { get; }

        public string FileName { get; }

        public List<Tag> Tags { get; }

        public bool Equals(IModel other)
        {
            return other != null &&
                other.FullPath == FullPath;
        }

        public override int GetHashCode()
        {
            return (FullPath).GetHashCode();
        }
    }
}
