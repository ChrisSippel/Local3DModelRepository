using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Local3DModelRepository.Wrappers
{
    internal sealed class StreamWrapper : IStreamWrapper
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

        public Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _stream.WriteAsync(buffer, offset, count, cancellationToken);
    }
}