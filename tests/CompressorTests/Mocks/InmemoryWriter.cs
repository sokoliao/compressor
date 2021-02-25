using CompressorLib;
using CompressorLib.Abstractions.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorTests.Mocks
{
    public class InmemoryWriter : IFileWriter
    {
        private readonly byte[] data;

        private int position = 0;

        public InmemoryWriter(byte[] data)
        {
            this.data = data;
        }

        public void Dispose() { }

        public void Write(byte[] buffer, int bufferOffset, int bufferSize)
        {
            for (var i = 0; i < bufferSize; i++)
            {
                data[position] = buffer[bufferOffset + i];
                position++;
            }
        }

        public void Write(byte[] buffer, int bufferOffset, int bufferSize, long fileOffset)
        {
            position = (int)fileOffset;
            Write(buffer, bufferOffset, bufferSize);
        }
    }
}
