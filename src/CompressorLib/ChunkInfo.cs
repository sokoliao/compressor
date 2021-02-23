using System;

namespace CompressorLib
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
    }
}
