using System.Collections.Generic;

namespace Local3DModelRepository.Models
{
    public interface IModel
    {
        string FileName { get; }

        string FullPath { get; }

        List<ITag> Tags { get; }
    }
}
