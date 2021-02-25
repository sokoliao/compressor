using CompressorLib;
using CompressorLib.Abstractions.Decompression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorTests.Mocks
{
    public class PassThroughDecompressor : IDecompressor
    {
        public int Decompress(
            byte[] inputBuffer, 
            int inputBufferOffset, 
            int inputBufferSize, 
            byte[] decompressionBuffer)
        {
            for (var i = 0; i < inputBufferSize; i++)
            {
                decompressionBuffer[i] = inputBuffer[i + inputBufferOffset];
            }
            return inputBufferSize;
        }
    }
}
