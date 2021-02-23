using CompressorLib;
using Xunit;

namespace CompressorTests
{
  public class ChunkSlicerTests
  {
    [Theory]
    [InlineData(10, 10, 1)]
    [InlineData(3, 10, 4)]
    [InlineData(5, 10, 2)]
    public void ShouldCalculateCount(int size, long length, long count)
    {
      Assert.Equal(count, ChunkSlicer.CalculateCount(size, length));
    }

    [Fact]
    public void ShouldSlice()
    {
      // Arrange

      var slicer = new ChunkSlicer(3, 8);
      var chunks = new ChunkInfo[4];
      var results = new bool[4];

      // Act

      for (var i = 0; i < 4; i++)
      {
        results[i] = slicer.TryGetChunk(out chunks[i]);
      }
      
      // Assert

      Assert.True(results[0]);
      Assert.Equal(0, chunks[0].offset);
      Assert.Equal(3, chunks[0].size);

      Assert.True(results[1]);
      Assert.Equal(3, chunks[1].offset);
      Assert.Equal(3, chunks[1].size);

      Assert.True(results[2]);
      Assert.Equal(6, chunks[2].offset);
      Assert.Equal(2, chunks[2].size);

      Assert.False(results[3]);
    }
  }
}