using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Abstractions.Shared
{
    public struct ChunkInfo
    {
        public long offset;
        public int size;
        public ChunkInfo(long offset, int size)
        {
            this.offset = offset;
            this.size = size;
        }
        public static ChunkInfo FromBinary(byte[] buffer, int offset)
        {
            return new ChunkInfo(
                BitConverter.ToInt64(buffer, offset),
                BitConverter.ToInt32(buffer, offset + 8));
        }
        public static void ToBinary(
            ChunkInfo info,
            byte[] buffer,
            int offset)
        {
            Array.Copy(BitConverter.GetBytes(info.offset), 0, buffer, offset, 8);
            Array.Copy(BitConverter.GetBytes(info.size), 0, buffer, offset + 8, 4);
        }
        public const int SIZE = 12;
    }
}
