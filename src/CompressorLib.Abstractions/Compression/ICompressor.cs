using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Abstractions.Compression
{
    public interface ICompressor
    {
        int Compress(
            byte[] inputBuffer,
            int inputBufferOffset,
            int inputBufferSize,
            byte[] compressionBuffer,
            int compressionBufferOffset,
            int compressionBufferSize);
    }
}
