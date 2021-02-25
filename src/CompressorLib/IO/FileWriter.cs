using CompressorLib.Abstractions.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class FileWriter : IFileWriter
    {
        private readonly Lazy<FileStream> writer;

        public FileWriter(string path)
        {
            writer = new Lazy<FileStream>(() =>
                new FileStream(path, FileMode.Append, FileAccess.Write));
        }

        public void Dispose()
        {
            if (writer.IsValueCreated)
            {
                writer.Value.Dispose();
            }
        }

        public void Write(
            byte[] buffer,
            int bufferOffset,
            int bufferSize)
        {
            writer.Value.Write(buffer, bufferOffset, bufferSize);
        }

        public void Write(
            byte[] buffer,
            int bufferOffset,
            int bufferSize,
            long fileOffset)
        {
            writer.Value.Seek(fileOffset, SeekOrigin.Begin);
            writer.Value.Write(buffer, bufferOffset, bufferSize);
        }
    }
}
