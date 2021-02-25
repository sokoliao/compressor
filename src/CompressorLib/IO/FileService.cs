using CompressorLib.Abstractions.IO;
using System.IO;

namespace CompressorLib
{
    public class FileService : IFileService
    {
        public bool Exists(string filePath) => File.Exists(filePath);

        public long GetSize(string filePath) => new FileInfo(filePath).Length;
    }
}