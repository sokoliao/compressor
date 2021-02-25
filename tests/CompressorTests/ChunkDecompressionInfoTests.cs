using CompressorLib;
using CompressorLib.Abstractions.Decompression;
using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests
{
    public class ChunkDecompressionInfoTests
    {
        [Theory]
        [InlineData(0L, 0, 0L, 0)]
        [InlineData(99L, 99, 100L, 100)]
        [InlineData(long.MaxValue, int.MaxValue, long.MaxValue, int.MaxValue)]
        public void ShouldSerializeDeserialize(long offset1, int size1, long offset2, int size2)
        {
            // Arrange

            var buffer = new byte[ChunkDecompressionInfo.SIZE];

            var input = new ChunkDecompressionInfo(
                new ChunkInfo(offset1, size1),
                new ChunkInfo(offset2, size2));

            // Act 

            ChunkDecompressionInfo.ToBinary(input, buffer, 0);
            var result = ChunkDecompressionInfo.FromBinary(buffer, 0);

            // Assert

            Assert.Equal(offset1, result.source.offset);
            Assert.Equal(size1, result.source.size);
            Assert.Equal(offset2, result.target.offset);
            Assert.Equal(size2, result.target.size);
        }
    }
}
