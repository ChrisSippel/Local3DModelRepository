using System;
using System.Collections.Generic;
using System.IO;

namespace Local3DModelRepository.Models
{
    public sealed class Model : IModel, IEquatable<IModel>
    {
        private readonly List<ITag> _tags;

        public Model(string fullPath)
            : this(fullPath, Path.GetFileName(fullPath), new List<ITag>())
        {
        }

        public Model(string fullPath, string fileName, List<ITag> tags)
        {
            _tags = tags;

            FullPath = fullPath;
            FileName = fileName;
            Tags = _tags;
        }

        public string FullPath { get; }

        public string FileName { get; }

        public List<ITag> Tags { get; }

        public bool Equals(IModel other)
        {
            return other != null &&
                other.FullPath == FullPath;
        }

        public override int GetHashCode()
        {
            return FullPath.GetHashCode();
        }
    }
}
