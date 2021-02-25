using CompressorLib;
using CompressorLib.Abstractions.Compressions;
using CompressorLib.Abstractions.IO;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests
{
    public class CompressionInfoGeneratorTests
    {
        [Fact]
        public void ShouldGenerateWhenFewChunks()
        {
            // Arrange

            var reader = new Mock<IFileService>();
            reader.Setup(m => m.GetSize(It.IsAny<string>())).Returns(20);

            var generator = new CompressionInfoGenerator(
                new CompressorInfoGeenratorOptions { Size = 10 },
                reader.Object);

            // Act

            var chunks = new List<ChunkCompressionInfo>();
            while (generator.TryDequeue(out var chunk))
            {
                chunks.Add(chunk);
            }

            // Assert

            Assert.Collection(chunks,
                first =>
                {
                    Assert.Equal(0L, first.source.offset);
                    Assert.Equal(10, first.source.size);
                },
                second =>
                {
                    Assert.Equal(10L, second.source.offset);
                    Assert.Equal(10, second.source.size);
                });
        }

        [Fact]
        public void ShouldGenerateWhenFewChunksAndLengthIsSmaller()
        {
            // Arrange

            var reader = new Mock<IFileService>();
            reader.Setup(m => m.GetSize(It.IsAny<string>())).Returns(12);

            var generator = new CompressionInfoGenerator(
                new CompressorInfoGeenratorOptions { Size = 10 },
                reader.Object);

            // Act

            var chunks = new List<ChunkCompressionInfo>();
            while (generator.TryDequeue(out var chunk))
            {
                chunks.Add(chunk);
            }

            // Assert

            Assert.Collection(chunks,
                first =>
                {
                    Assert.Equal(0L, first.source.offset);
                    Assert.Equal(10, first.source.size);
                },
                second =>
                {
                    Assert.Equal(10L, second.source.offset);
                    Assert.Equal(2, second.source.size);
                });
        }

        [Fact]
        public void ShouldGenerateWhenOneChunkAndLengthIsSmaller()
        {
            // Arrange

            var reader = new Mock<IFileService>();
            reader.Setup(m => m.GetSize(It.IsAny<string>())).Returns(2);

            var generator = new CompressionInfoGenerator(
                new CompressorInfoGeenratorOptions { Size = 10 },
                reader.Object);

            // Act

            var chunks = new List<ChunkCompressionInfo>();
            while (generator.TryDequeue(out var chunk))
            {
                chunks.Add(chunk);
            }

            // Assert

            Assert.Collection(chunks,
                first =>
                {
                    Assert.Equal(0L, first.source.offset);
                    Assert.Equal(2, first.source.size);
                });
        }

        [Fact]
        public void ShouldGenerateWhenOneChunk()
        {
            // Arrange

            var reader = new Mock<IFileService>();
            reader.Setup(m => m.GetSize(It.IsAny<string>())).Returns(10);

            var generator = new CompressionInfoGenerator(
                new CompressorInfoGeenratorOptions { Size = 10 },
                reader.Object);

            // Act

            var chunks = new List<ChunkCompressionInfo>();
            while (generator.TryDequeue(out var chunk))
            {
                chunks.Add(chunk);
            }

            // Assert

            Assert.Collection(chunks,
                first =>
                {
                    Assert.Equal(0L, first.source.offset);
                    Assert.Equal(10, first.source.size);
                });
        }

        [Fact]
        public void ShouldNotGenerateWhenEmpty()
        {
            // Arrange

            var reader = new Mock<IFileService>();
            reader.Setup(m => m.GetSize(It.IsAny<string>())).Returns(0);

            var generator = new CompressionInfoGenerator(
                new CompressorInfoGeenratorOptions { Size = 10 },
                reader.Object);

            // Act

            var chunks = new List<ChunkCompressionInfo>();
            while (generator.TryDequeue(out var chunk))
            {
                chunks.Add(chunk);
            }

            // Assert

            Assert.Empty(chunks);
        }
    }
}
