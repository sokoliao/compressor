using CompressorLib.Abstractions.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class FileReader : IFileReader
    {
        private readonly Lazy<FileStream> reader;
        private readonly Lazy<FileInfo> info;

        public FileReader(string path)
        {
            reader = new Lazy<FileStream>(() => 
                new FileStream(path, FileMode.Open, FileAccess.Read));
            info = new Lazy<FileInfo>(() => new FileInfo(path));
        }

        public void Dispose()
        {
            if (reader.IsValueCreated)
            {
                reader.Value.Dispose();
            }
        }

        public bool Exists() => info.Value.Exists;

        public long Length() => info.Value.Length;

        public void Read(byte[] buffer, int bufferOffset, int bufferSize, long fileOffset)
        {
            reader.Value.Seek(fileOffset, SeekOrigin.Begin);
            reader.Value.Read(buffer, bufferOffset, bufferSize);
        }
    }
}
