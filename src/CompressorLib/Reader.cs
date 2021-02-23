using System.IO;
using System.IO.MemoryMappedFiles;

namespace CompressorLib
{
  public class Reader : IReader
  {
    public byte[] ReadChunk(string filePath, long offset, int size)
    {
      // var buffer = new byte[size];
      // using (var mmf = MemoryMappedFile.CreateFromFile(filePath))
      // {
      //   using (var accessor = mmf.CreateViewAccessor(offset, size, MemoryMappedFileAccess.Read))
      //   {
      //     accessor.ReadArray(offset, buffer, 0, size);
      //   }
      // }
      // return buffer;
      var buffer = new byte[size];
      using (var reader = new FileStream(filePath, FileMode.Open))
      {
        reader.Seek(offset, SeekOrigin.Begin);
        reader.Read(buffer, 0, size);
        return buffer;
      }
    }
  }
  public interface IReader
  {
    byte[] ReadChunk(string filePath, long offset, int size);
  }
}