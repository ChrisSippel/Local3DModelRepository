using System;
using System.Threading;
using System.Threading.Tasks;

namespace Local3DModelRepository.FileSystemAccess
{
    public interface IStreamWrapper : IDisposable
    {
        ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken);
    }
}
