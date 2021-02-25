using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Abstractions.IO
{
    public interface IFileReader : IDisposable
    {
        bool Exists();
        long Length();
        void Read(byte[] buffer, int bufferOffset, int bufferSize, long fileOffse);
    }
}
