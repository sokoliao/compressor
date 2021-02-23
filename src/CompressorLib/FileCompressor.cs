using System;
using System.IO;
using System.IO.Compression;

namespace CompressorLib
{
  public class FileCompressor : IFileCompressor
  {
    private readonly IFileService fileService;
    private readonly IReader reader;
    private readonly ICompressor compressor;
    private readonly IChunkWriter writer;

    public FileCompressor(
      IFileService fileService,
      IReader reader,
      ICompressor compressor,
      IChunkWriter writer)
    {
      this.fileService = fileService;
      this.reader = reader;
      this.compressor = compressor;
      this.writer = writer;
    }
    public void Compress(string source, int chunkSize, string destination)
    {
      var fileSize = fileService.GetSize(source);
      var chunkCount = fileSize % chunkSize;
      if (chunkCount * chunkSize < fileSize)
      {
        chunkCount++;
      }
      for (var i = 0; i < chunkCount; i++)
      {
        long offset = i * chunkSize;
        int size = (int)(Math.Min(fileSize, offset + chunkSize) - offset);

        var chunk = reader.ReadChunk(source, offset, size);
        var compressed = compressor.Compress(chunk);
        writer.Write(destination, new ChunkInfo(offset, size), compressed);
      }
    }
  }
  public interface IFileCompressor
  {
    void Compress(string source, int chunkSize, string destination);
  }
}