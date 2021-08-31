using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Local3DModelRepository.FileSystemAccess
{
    public sealed class StreamWrapper : IStreamWrapper
    {
        private readonly Stream _stream;

        public StreamWrapper(Stream stream)
        {
            _stream = stream;
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken) => _stream.WriteAsync(buffer, cancellationToken);
    }
}
