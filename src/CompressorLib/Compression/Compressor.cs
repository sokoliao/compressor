using CompressorLib.Abstractions.Compression;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class Compressor : ICompressor
    {
        public int Compress(
            byte[] inputBuffer,
            int inputBufferOffset,
            int inputBufferSize,
            byte[] compressionBuffer,
            int compressionBufferOffset,
            int compressionBufferSize)
        {
            var compressed = new MemoryStream(compressionBuffer, compressionBufferOffset, compressionBufferSize);
            using (var compressor = new GZipStream(compressed, CompressionMode.Compress, true))
            {
                compressor.Write(inputBuffer, inputBufferOffset, inputBufferSize);
            }
            return (int)compressed.Position;
        }
    }
}
