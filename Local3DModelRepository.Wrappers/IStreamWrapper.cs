using System;
using System.Threading;
using System.Threading.Tasks;

namespace Local3DModelRepository.Wrappers
{
    public interface IStreamWrapper : IDisposable
    {
        Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
    }
}