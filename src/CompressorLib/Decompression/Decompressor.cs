using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class Decompressor : IDecompressor
    {
        public int Decompress(
            byte[] inputBuffer, 
            int inputBufferOffset, 
            int inputBufferSize, 
            byte[] decompressionBuffer)
        {
            var compressed = new MemoryStream(inputBuffer);
            using (var decompressor = new GZipStream(compressed, CompressionMode.Decompress, true))
            {
                return decompressor.Read(decompressionBuffer, 0, decompressionBuffer.Length);
            }
        }
    }

    public interface IDecompressor
    {
        int Decompress(
            byte[] inputBuffer,
            int inputBufferOffset,
            int inputBufferSize,
            byte[] decompressionBuffer);
    }
}
