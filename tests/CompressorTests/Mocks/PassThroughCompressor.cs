using CompressorLib;
using CompressorLib.Abstractions.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorTests.Mocks
{
    public class PassThroughCompressor : ICompressor
    {
        public int Compress(
            byte[] inputBuffer, 
            int inputBufferOffset, 
            int inputBufferSize, 
            byte[] compressionBuffer, 
            int compressionBufferOffset, 
            int compressionBufferSize)
        {
            Array.Copy(inputBuffer, inputBufferOffset, compressionBuffer, compressionBufferOffset, inputBufferSize);
            return inputBufferSize;
        }
    }
}
