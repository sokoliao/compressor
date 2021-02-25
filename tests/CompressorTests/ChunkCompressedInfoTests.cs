using CompressorLib;
using CompressorLib.Abstractions.Compression;
using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests
{
    public class ChunkCompressedInfoTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 100, 100)]
        [InlineData(0, long.MaxValue, int.MaxValue)]
        [InlineData(100, 0, 0)]
        [InlineData(100, 100, 100)]
        [InlineData(100, long.MaxValue, int.MaxValue)]
        [InlineData(int.MaxValue, 0, 0)]
        [InlineData(int.MaxValue, 100, 100)]
        [InlineData(int.MaxValue, long.MaxValue, int.MaxValue)]
        public void ShouldSerializeDeserialize(int size1, long offset2, int size2)
        {
            // Arrange

            var buffer = new byte[ChunkCompressedInfo.SIZE];

            var input = new ChunkCompressedInfo(size1, new ChunkInfo(offset2, size2));

            // Act 

            ChunkCompressedInfo.ToBinary(input, buffer, 0);
            var result = ChunkCompressedInfo.FromBinary(buffer, 0);

            // Assert

            Assert.Equal(size1, result.size);
            Assert.Equal(offset2, result.target.offset);
            Assert.Equal(size2, result.target.size);
        }
    }
}
