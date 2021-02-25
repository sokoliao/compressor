using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Abstractions.Compression
{
    public struct ChunkCompressedInfo
    {
        public int size;
        public ChunkInfo target;
        public ChunkCompressedInfo(int size, ChunkInfo target)
        {
            this.size = size;
            this.target = target;
        }
        public static void ToBinary(
            ChunkCompressedInfo info,
            byte[] buffer,
            int offset)
        {
            Array.Copy(BitConverter.GetBytes(info.size), 0, buffer, offset, 4);
            ChunkInfo.ToBinary(info.target, buffer, offset + 4);
        }

        public static ChunkCompressedInfo FromBinary(
            byte[] buffer,
            int offset)
        {
            return new ChunkCompressedInfo(
                BitConverter.ToInt32(buffer, offset),
                ChunkInfo.FromBinary(buffer, offset + 4));
        }

        public const int SIZE = 16;
    }
}
