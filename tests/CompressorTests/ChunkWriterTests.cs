using System;
using System.IO;
using CompressorLib;
using Xunit;

namespace CompressorTests
{
  public class ChunkWriterTests : IClassFixture<TempFileFixture>
  {
    private readonly TempFileFixture tempFileFixture;

    public ChunkWriterTests(TempFileFixture tempFileFixture)
    {
      this.tempFileFixture = tempFileFixture;
    }

    [Fact]
    public void ShouldWriteChunkAndInfo()
    {
      // Arrange

      var filePath = tempFileFixture.GetTempFile();
      var writer = new ChunkWriter();

      // Act

      writer.Write(filePath, new ChunkInfo(0, 5), new byte[] { 1, 2, 3, 4, 5 });

      var offset = new byte[8];
      var size = new byte[4];
      var data = new byte[5];
      using var reader = new FileStream(filePath, FileMode.Open);
      reader.Read(offset);
      reader.Read(size);
      reader.Read(data);

      // Assert

      Assert.Equal(17, reader.Length);
      Assert.Equal(0, BitConverter.ToInt64(offset));
      Assert.Equal(5, BitConverter.ToInt32(size));
      Assert.Equal(new byte[] { 1, 2, 3, 4, 5 }, data);
    }
  }
}