using CompressorLib;
using CompressorLib.Abstractions.Decompression;
using CompressorTests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests
{
    public class DecompressionInfoGeneratorTests
    {
        [Fact]
        public void ShouldGenerateWhenFewChunks()
        {
            //  Arrange

            var file = new byte[16 + 5 + 16 + 5]
            {
                /* compressedSize */ 5, 0, 0, 0, /*  source.offset */ 0, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 10, 0, 0, 0,
                1, 2, 3, 4, 5, 
                /* compressedSize */ 5, 0, 0, 0, /* source.offset */ 10, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 7, 0, 0, 0,
                6, 7, 8, 9, 10,
            };
            var reader = new InmemoryReader(file);

            var generator = new DecompressionInfoGenerator(reader);

            // Act

            var chunks = new List<ChunkDecompressionInfo>();
            while (generator.TryDequeue(out var info))
            {
                chunks.Add(info);
            }

            // Assert

            Assert.Collection(chunks,
                info =>
                {
                    Assert.Equal(16L, info.source.offset);
                    Assert.Equal(5, info.source.size);
                    Assert.Equal(0L, info.target.offset);
                    Assert.Equal(10, info.target.size);
                },
                info =>
                {
                    Assert.Equal(37L, info.source.offset);
                    Assert.Equal(5, info.source.size);
                    Assert.Equal(10L, info.target.offset);
                    Assert.Equal(7, info.target.size);
                });
        }

        [Fact]
        public void ShouldGenerateWhenFewChunksAndLengthIsSmaller()
        {
            //  Arrange

            var file = new byte[16 + 5 + 16 + 3]
            {
                /* compressedSize */ 5, 0, 0, 0, /*  source.offset */ 0, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 10, 0, 0, 0,
                1, 2, 3, 4, 5, 
                /* compressedSize */ 3, 0, 0, 0, /* source.offset */ 10, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 7, 0, 0, 0,
                6, 7, 8
            };
            var reader = new InmemoryReader(file);

            var generator = new DecompressionInfoGenerator(reader);

            // Act

            var chunks = new List<ChunkDecompressionInfo>();
            while (generator.TryDequeue(out var info))
            {
                chunks.Add(info);
            }

            // Assert

            Assert.Collection(chunks,
                info =>
                {
                    Assert.Equal(16L, info.source.offset);
                    Assert.Equal(5, info.source.size);
                    Assert.Equal(0L, info.target.offset);
                    Assert.Equal(10, info.target.size);
                },
                info =>
                {
                    Assert.Equal(37L, info.source.offset);
                    Assert.Equal(3, info.source.size);
                    Assert.Equal(10L, info.target.offset);
                    Assert.Equal(7, info.target.size);
                });
        }

        [Fact]
        public void ShouldGenerateWhenOneChunkAndLengthIsSmaller()
        {
            //  Arrange

            var file = new byte[16 + 3]
            {
                /* compressedSize */ 3, 0, 0, 0, /*  source.offset */ 0, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 10, 0, 0, 0,
                1, 2, 3, 
            };
            var reader = new InmemoryReader(file);

            var generator = new DecompressionInfoGenerator(reader);

            // Act

            var chunks = new List<ChunkDecompressionInfo>();
            while (generator.TryDequeue(out var info))
            {
                chunks.Add(info);
            }

            // Assert

            Assert.Collection(chunks,
                info =>
                {
                    Assert.Equal(16L, info.source.offset);
                    Assert.Equal(3, info.source.size);
                    Assert.Equal(0L, info.target.offset);
                    Assert.Equal(10, info.target.size);
                });
        }

        [Fact]
        public void ShouldGenerateWhenOneChunk()
        {
            //  Arrange

            var file = new byte[16 + 5]
            {
                /* compressedSize */ 5, 0, 0, 0, /*  source.offset */ 0, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 10, 0, 0, 0,
                1, 2, 3, 4, 5, 
            };
            var reader = new InmemoryReader(file);

            var generator = new DecompressionInfoGenerator(reader);

            // Act

            var chunks = new List<ChunkDecompressionInfo>();
            while (generator.TryDequeue(out var info))
            {
                chunks.Add(info);
            }

            // Assert

            Assert.Collection(chunks,
                info =>
                {
                    Assert.Equal(16L, info.source.offset);
                    Assert.Equal(5, info.source.size);
                    Assert.Equal(0L, info.target.offset);
                    Assert.Equal(10, info.target.size);
                });
        }

        [Fact]
        public void ShouldNotGenerateWhenEmpty()
        {
            //  Arrange

            var file = new byte[0];
            var reader = new InmemoryReader(file);

            var generator = new DecompressionInfoGenerator(reader);

            // Act

            var chunks = new List<ChunkDecompressionInfo>();
            while (generator.TryDequeue(out var info))
            {
                chunks.Add(info);
            }

            // Assert

            Assert.Empty(chunks);
        }
    }
}
