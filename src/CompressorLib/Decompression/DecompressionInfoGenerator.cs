using CompressorLib.Abstractions.Compression;
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
    public class DecompressionInfoGenerator : IGenerator<ChunkDecompressionInfo>
    {
        private readonly IFileReader reader;
        private readonly byte[] infoBuffer = new byte[ChunkCompressedInfo.SIZE];
        private long position = 0;
        private readonly Lazy<long> length;
        public DecompressionInfoGenerator(
            IFileReader reader)
        {
            this.reader = reader;
            length = new Lazy<long>(() => reader.Length());
        }

        public bool TryDequeue(out ChunkDecompressionInfo info)
        {
            if (position >= length.Value)
            {
                info = default;
                return false;
            }
            reader.Read(infoBuffer, 0, ChunkCompressedInfo.SIZE, position);
            position += ChunkCompressedInfo.SIZE;
            var compressedInfo = ChunkCompressedInfo.FromBinary(infoBuffer, 0);
            
            info = new ChunkDecompressionInfo(
                new ChunkInfo(position, compressedInfo.size),
                compressedInfo.target);
            position += compressedInfo.size;
            return true;
        }
    }
}
