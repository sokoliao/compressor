using System.IO;
using System.IO.Compression;

namespace CompressorLib
{
  public class Compressor : ICompressor
  {
    public byte[] Compress(byte[] data)
    {
      var compressed = new MemoryStream();
      using (var compressor = new GZipStream(compressed, CompressionMode.Compress, true))
      {
        compressor.Write(data, 0, data.Length);
      }
      return compressed.GetBuffer();
    }
  }
  public interface ICompressor
  {
    byte[] Compress(byte[] data);
  }
}