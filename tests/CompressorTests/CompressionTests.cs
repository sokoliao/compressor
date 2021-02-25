using CompressorLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests
{
    public class CompressionTests
    {
        [Fact]
        public void ShouldCompressAndDecompress()
        {
            // Arrange

            var size = 100;

            var data = new byte[size];
            new Random(100500).NextBytes(data);

            var compressionBuffer = new byte[size * 2];
            var compressor = new Compressor();

            var decompressionBuffer = new byte[size * 2];
            var decompressor = new Decompressor();

            // Act

            var compressedSize = compressor.Compress(data, 0, size, compressionBuffer, 0, compressionBuffer.Length);
            var decompressedSize = decompressor.Decompress(compressionBuffer, 0, compressedSize, decompressionBuffer);

            // Assert

            Assert.Equal(data, decompressionBuffer.Take(decompressedSize));
        }
    }
}
