using System;
using System.IO;

namespace CompressorLib
{
  public class ChunkWriter : IChunkWriter
  {
    public void Write(string filePath, ChunkInfo info, byte[] data)
    {
      using (var writer = new FileStream(filePath, FileMode.Append, FileAccess.Write))
      {
        var offset = writer.Position;
        writer.Write(BitConverter.GetBytes(info.offset), 0, 8);
        writer.Write(BitConverter.GetBytes(info.size), 0, 4);
        writer.Write(data, 0, data.Length);
      }
    }
  }
  public interface IChunkWriter
  {
    void Write(string filePath, ChunkInfo info, byte[] data);
  }
}