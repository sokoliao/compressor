using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Abstractions.IO
{
    public interface IFileWriter : IDisposable
    {
        void Write(
            byte[] buffer,
            int bufferOffset,
            int bufferSize);

        void Write(
            byte[] buffer,
            int bufferOffset,
            int bufferSize,
            long fileOffset);
    }
}
