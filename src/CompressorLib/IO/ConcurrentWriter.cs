using CompressorLib.Abstractions.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class ConcurrentWriter : IFileWriter
    {
        private readonly IFileWriter decoratee;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public ConcurrentWriter(IFileWriter decoratee)
        {
            this.decoratee = decoratee;
        }

        public void Dispose() => decoratee.Dispose();

        public void Write(byte[] buffer, int bufferOffset, int bufferSize)
        {
            semaphore.Wait();
            try
            {
                decoratee.Write(buffer, bufferOffset, bufferSize);
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void Write(byte[] buffer, int bufferOffset, int bufferSize, long fileOffset)
        {
            semaphore.Wait();
            try
            {
                decoratee.Write(buffer, bufferOffset, bufferSize, fileOffset);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
