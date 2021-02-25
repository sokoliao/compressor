using CompressorLib;
using CompressorTests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests
{
    public class FileDecompressorTests
    {
        [Fact]
        public void ShouldDecompress()
        {
            // Arrange

            var file = new byte[16 + 10 + 16 + 2]
            {
                /* compressedSize */ 10, 0, 0, 0, /*  source.offset */ 0, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 10, 0, 0, 0,
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                /* compressedSize */ 2, 0, 0, 0, /* source.offset */ 10, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 2, 0, 0, 0,
                11, 12
            };
            var reader = new InmemoryReader(file);

            var decompressor = new PassThroughDecompressor();

            var output = new byte[12];
            var writer = new InmemoryWriter(output);

            var fileDecompressor = new FileDecompressor(
                new DecompressionInfoGenerator(reader),
                reader,
                decompressor,
                writer);

            // Act

            fileDecompressor.Execute();

            // Assert

            Assert.Equal(Enumerable.Range(1, 12).Select(i => (byte)i), output);
        }
    }
}
