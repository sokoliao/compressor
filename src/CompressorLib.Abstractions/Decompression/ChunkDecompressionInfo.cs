using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorLib.Abstractions.Decompression
{
    public struct ChunkDecompressionInfo
    {
        public ChunkInfo source;
        public ChunkInfo target;
        public ChunkDecompressionInfo(ChunkInfo source, ChunkInfo target)
        {
            this.source = source;
            this.target = target;
        }
        public static void ToBinary(
            ChunkDecompressionInfo info,
            byte[] buffer,
            int offset)
        {
            ChunkInfo.ToBinary(info.source, buffer, offset);
            ChunkInfo.ToBinary(info.target, buffer, offset + ChunkInfo.SIZE);
        }

        public static ChunkDecompressionInfo FromBinary(
            byte[] buffer,
            int offset)
        {
            return new ChunkDecompressionInfo(
                ChunkInfo.FromBinary(buffer, offset),
                ChunkInfo.FromBinary(buffer, offset + ChunkInfo.SIZE));
        }

        public const int SIZE = 24;
    }
}
