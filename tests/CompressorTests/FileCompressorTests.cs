using System;
using Xunit;
using CompressorLib;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace CompressorTests
{
  public class FileCompressorTests : IClassFixture<TempFileFixture>
  {
    private readonly TempFileFixture tempFileFixture;

    public FileCompressorTests(TempFileFixture tempFileFixture)
    {
      this.tempFileFixture = tempFileFixture;
    }
    [Fact]
    public void ShouldCompressFileInChunks()
    {
      // Arrange

      const string SOURCE = @"C:\file.txt";
      const string DESTINATION = @"C:\file.txt.gz";

      var chunkSize = 5;

      var fileService = new Mock<IFileService>();
      fileService
        .Setup(fs => fs.GetSize(It.IsAny<string>()))
        .Returns(12);

      var reader = new Mock<IReader>();
      reader
        .SetupSequence(r => r.ReadChunk(
          It.IsAny<string>(), 
          It.IsAny<long>(), 
          It.IsAny<int>()))
        .Returns(new byte[] { 1, 2, 3, 4, 5 })
        .Returns(new byte[] { 6, 7, 8, 9, 10 })
        .Returns(new byte[] { 11, 12 });

      var compressor = new Mock<ICompressor>();
      compressor
        .SetupSequence(c => c.Compress(It.IsAny<byte[]>()))
        .Returns(new byte[] { 1, 1 })
        .Returns(new byte[] { 2, 3, 5 })
        .Returns(new byte[] { 8 });

      var result = new List<byte[]>();
      var writer = new Mock<IChunkWriter>();
      writer
        .Setup(w => w.Write(It.IsAny<string>(), It.IsAny<ChunkInfo>(), It.IsAny<byte[]>()))
        .Callback((string _, ChunkInfo _, byte[] arg) => result.Add(arg));

      var worker = new FileCompressor(
        fileService.Object,
        reader.Object,
        compressor.Object,
        writer.Object);

                
      // Act

      worker.Compress(SOURCE, chunkSize, DESTINATION);
      
      // Assert

      Assert.Equal(new byte[6] { 1, 1, 2, 3, 5, 8 }, result.SelectMany(x => x));

      fileService.Verify(fs => fs.GetSize(It.Is<string>(p => p == SOURCE)), Times.Once);

      reader.Verify(r => 
        r.ReadChunk(
          It.Is<string>(p => p == SOURCE),
          It.Is<long>(p => p == 0),
          It.Is<int>(p => p == 5)),
        Times.Once);
      reader.Verify(r => 
        r.ReadChunk(
          It.Is<string>(p => p == SOURCE),
          It.Is<long>(p => p == 5),
          It.Is<int>(p => p == 5)),
        Times.Once);
      reader.Verify(r => 
        r.ReadChunk(
          It.Is<string>(p => p == SOURCE),
          It.Is<long>(p => p == 10),
          It.Is<int>(p => p == 2)),
        Times.Once);
      reader.VerifyNoOtherCalls();

      compressor.Verify(
        c => c.Compress(It.Is<byte[]>(p => p.SequenceEqual(new byte[] { 1, 2, 3, 4, 5 }))),
        Times.Once);
      compressor.Verify(
        c => c.Compress(It.Is<byte[]>(p => p.SequenceEqual(new byte[] { 6, 7, 8, 9, 10 }))),
        Times.Once);
      compressor.Verify(
        c => c.Compress(It.Is<byte[]>(p => p.SequenceEqual(new byte[] { 11, 12 }))),
        Times.Once);
      compressor.VerifyNoOtherCalls();

      writer.Verify(
        w => w.Write(
          It.Is<string>(p => p == DESTINATION), 
          It.Is<ChunkInfo>(p => p.offset == 0 && p.size == 5),
          It.Is<byte[]>(p => p.SequenceEqual(new byte[] { 1, 1 }))),
        Times.Once);
      writer.Verify(
        w => w.Write(
          It.Is<string>(p => p == DESTINATION),
          It.Is<ChunkInfo>(p => p.offset == 5 && p.size == 5),
          It.Is<byte[]>(p => p.SequenceEqual(new byte[] { 2, 3, 5 }))),
        Times.Once);
      writer.Verify(
        w => w.Write(
          It.Is<string>(p => p == DESTINATION),
          It.Is<ChunkInfo>(p => p.offset == 10 && p.size == 2),
          It.Is<byte[]>(p => p.SequenceEqual(new byte[] { 8 }))),
        Times.Once);
      writer.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldCompressFile()
    {
      // Arrange

      var inputFilePath = tempFileFixture.GetTempFile();
      using (var writer = new FileStream(inputFilePath, FileMode.Create))
      {
        writer.Write(new byte[10] { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55 });
      }
      var outputFilePath = tempFileFixture.GetTempFile();

      var compressor = new FileCompressor(
        new FileService(),
        new Reader(),
        new PassThroughCompressor(),
        new ChunkWriter()
      );

      // Act

      compressor.Compress(inputFilePath, 4, outputFilePath);

      // Assert

      using (var reader = new FileStream(outputFilePath, FileMode.Open))
      {
        Assert.Equal(46, reader.Length);

        var output = new byte[46];
        reader.Read(output);
        Assert.Equal(
          new byte[46] { 
            0, 0, 0, 0, 0, 0, 0, 0,
            4, 0, 0, 0,
            1, 1, 2, 3, 
            
            4, 0, 0, 0, 0, 0, 0, 0,
            4, 0, 0, 0,
            5, 8, 13, 21,

            8, 0, 0, 0, 0, 0, 0, 0,
            2, 0, 0, 0,
            34, 55
          },
          output
        );
      }
    }
  }
}
