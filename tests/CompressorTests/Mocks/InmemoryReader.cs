using CompressorLib;
using CompressorLib.Abstractions.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorTests.Mocks
{
    class InmemoryReader : IFileReader
    {
        private readonly byte[] data;

        public InmemoryReader(byte[] data)
        {
            this.data = data;
        }

        public void Dispose() { }

        public bool Exists() => true;

        public long Length() => data.Length;

        public void Read(byte[] buffer, int bufferOffset, int bufferSize, long fileOffse)
        {
            for (var i = 0; i < bufferSize; i++)
            {
                buffer[bufferOffset + i] = data[(int)fileOffse + i];
            }
        }
    }
}
