using CompressorLib.Abstractions.Compression;
using CompressorLib.Abstractions.Compressions;
using CompressorLib.Abstractions.IO;
using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class FileCompressor : IFileCompressor
    {
        private readonly IGenerator<ChunkCompressionInfo> generator;
        private readonly IFileReader reader;
        private readonly ICompressor compressor;
        private readonly IFileWriter writer;
        private readonly int size;
        private readonly Lazy<byte[]> readBuffer;
        private readonly Lazy<byte[]> compressionBuffer;
        public FileCompressor(
            FileCompressorOptions  options,
            IGenerator<ChunkCompressionInfo> generator,
            IFileReader reader,
            ICompressor compressor,
            IFileWriter writer)
        {
            size = options.Size;
            this.generator = generator;
            this.reader = reader;
            this.compressor = compressor;
            this.writer = writer;
            readBuffer = new Lazy<byte[]>(() => new byte[size]);
            compressionBuffer = new Lazy<byte[]>(() => 
                new byte[(ChunkCompressedInfo.SIZE + size) * 2]);
        }
        public void Compress()
        {
            while (generator.TryDequeue(out var chunk))
            {
                reader.Read(readBuffer.Value, 0, chunk.source.size, chunk.source.offset);

                var compressedSize = compressor.Compress(
                    readBuffer.Value, 0, chunk.source.size,
                    compressionBuffer.Value, ChunkCompressedInfo.SIZE, compressionBuffer.Value.Length - ChunkCompressedInfo.SIZE);

                var info = new ChunkCompressedInfo(
                    compressedSize,
                    chunk.source);
                ChunkCompressedInfo.ToBinary(info, compressionBuffer.Value, 0);

                writer.Write(compressionBuffer.Value, 0, ChunkCompressedInfo.SIZE + compressedSize);
            }
        }
    }

    public interface IFileCompressor
    {
        void Compress();
    }

    public class FileCompressorOptions
    {
        public int Size { get; set; }
    }
}
