using CompressorLib;
using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests
{
    public class ChunkInfoTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 100)]
        [InlineData(0, int.MaxValue)]
        [InlineData(100, 0)]
        [InlineData(100, 100)]
        [InlineData(100, int.MaxValue)]
        [InlineData(long.MaxValue, 0)]
        [InlineData(long.MaxValue, 100)]
        [InlineData(long.MaxValue, int.MaxValue)]
        public void ShouldSerializeDeserialize(long offset, int size)
        {
            var chunk = new ChunkInfo(offset, size);
            var buffer = new byte[ChunkInfo.SIZE];

            ChunkInfo.ToBinary(chunk, buffer, 0);
            var result = ChunkInfo.FromBinary(buffer, 0);

            Assert.Equal(offset, result.offset);
            Assert.Equal(size, result.size);
        }
    }
}
