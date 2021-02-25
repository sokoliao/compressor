using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests.Mocks
{
    public class InmemoryTests
    {
        [Fact]
        public void ShouldWrite()
        {
            // Arrange

            var data = new byte[100];
            new Random(100500).NextBytes(data);

            var file = new byte[100];
            var writer = new InmemoryWriter(file);

            // Act

            writer.Write(data, 0, data.Length);

            // Assert

            Assert.Equal(data, file);
        }

        [Fact]
        public void ShouldRead()
        {
            // Arrange

            var buffer = new byte[100];

            var file = new byte[100];
            new Random(100500).NextBytes(file);
            var reader = new InmemoryReader(file);

            // Act

            var length = reader.Length();

            reader.Read(buffer, 0, buffer.Length, 0);

            // Assert

            Assert.Equal(file, buffer);
            Assert.Equal(100L, length);
        }
    }
}
