using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests.Mocks
{
    public class PassThroughCompressorTests
    {
        [Fact]
        public void ShouldPassThrough()
        {
            // Arrange

            var input = new byte[1000];
            new Random(100500).NextBytes(input);

            var output = new byte[2000];

            var compressor = new PassThroughCompressor();

            // Act

            var size = compressor.Compress(input, 0, input.Length, output, 100, output.Length);

            // Assert

            Assert.Equal(1000, size);
            Assert.Equal(input, output.Skip(100).Take(1000));
        }
    }
}
