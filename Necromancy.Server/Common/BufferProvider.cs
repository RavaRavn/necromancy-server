using Arrowgene.Buffers;

namespace Necromancy.Server.Common
{
    public class BufferProvider
    {
        private static readonly IBufferProvider Provider = new StreamBuffer();

        public static IBuffer Provide()
        {
            return Provider.Provide();
        }

        public static IBuffer Provide(byte[] data)
        {
            return Provider.Provide(data);
        }
    }
}
