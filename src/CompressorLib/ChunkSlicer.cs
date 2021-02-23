using System;

namespace CompressorLib
{
  public class ChunkSlicer : IChunkSlicer
  {
    private readonly int size;
    private readonly long length;
    private int index;
    private bool initialized;
    private readonly Lazy<long> count;

    public ChunkSlicer(int size, long length)
    {
      this.size = size;
      this.length = length;
      this.index = 0;
      count = new Lazy<long>(() => CalculateCount(size, length));
    }
    public static long CalculateCount(int size, long length)
    {
      var count = length / size;
      if ((count * (long)size) < length)
        count++;
      return count;
    }
    public bool TryGetChunk(out ChunkInfo info)
    {
      if (index < count.Value)
      {
        var offset = index * size;
        var chunkSize = (int)(Math.Min(length, offset + size) - offset);
        info = new ChunkInfo(index * size, chunkSize);
        index++;
        return true;
      }
      else
      {
        info = new ChunkInfo();
        return false;
      }
    }
  }
  public interface IChunkSlicer
  {
    bool TryGetChunk(out ChunkInfo info);
  }
}