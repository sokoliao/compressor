namespace CompressorLib
{
  public class PassThroughCompressor : ICompressor
  {
    public byte[] Compress(byte[] data) => data;
  }
}