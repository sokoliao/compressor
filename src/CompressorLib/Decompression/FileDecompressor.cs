using CompressorLib.Abstractions.Decompression;
using CompressorLib.Abstractions.IO;
using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class FileDecompressor : IFileDecompressor
    {
        private readonly IGenerator<ChunkDecompressionInfo> generator;
        private readonly IFileReader reader;
        private readonly IDecompressor decompressor;
        private readonly IFileWriter writer;
        private byte[] readBuffer = new byte[0];
        private byte[] decompressBuffer = new byte[0];
        public FileDecompressor(
            IGenerator<ChunkDecompressionInfo> generator,
            IFileReader reader,
            IDecompressor decompressor,
            IFileWriter writer)
        {
            this.generator = generator;
            this.reader = reader;
            this.decompressor = decompressor;
            this.writer = writer;
        }
        public void Execute()
        {
            while (generator.TryDequeue(out var info))
            {
                if (info.source.size > readBuffer.Length)
                {
                    readBuffer = new byte[info.source.size];
                }
                if (info.target.size > decompressBuffer.Length)
                {
                    decompressBuffer = new byte[info.target.size];
                }

                reader.Read(readBuffer, 0, info.source.size, info.source.offset);

                var decompressedSize = decompressor.Decompress(readBuffer, 0, info.source.size, decompressBuffer);

                if (decompressedSize != info.target.size)
                {
                    throw new Exception();
                }

                writer.Write(decompressBuffer, 0, info.target.size, info.target.offset);
            }
        }
        public void Dispose() { }
    }
}
