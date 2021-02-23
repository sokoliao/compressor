using System.IO;

namespace CompressorLib
{
  public class Writer : IWriter
  {
    public void WriteChunk(string filePath, byte[] buffer)
    {
      using (var writer = new FileStream(filePath, FileMode.Append, FileAccess.Write))
      {
        writer.Write(buffer, 0, buffer.Length);
      }
    }
  }
  public interface IWriter
  {
    void WriteChunk(string filePath, byte[] buffer);
  }
}