using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Abstractions.Decompression
{
    public interface IDecompressor
    {
        int Decompress(
            byte[] inputBuffer,
            int inputBufferOffset,
            int inputBufferSize,
            byte[] decompressionBuffer);
    }
}
