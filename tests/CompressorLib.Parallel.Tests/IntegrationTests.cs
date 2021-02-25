using Autofac;
using CompressorLib.Abstractions.Compression;
using CompressorLib.Abstractions.Decompression;
using CompressorLib.Parallel.Creation;
using System;
using System.IO;
using Xunit;

namespace CompressorLib.Parallel.Tests
{
    public class IntegrationTests : IClassFixture<TempFileFixture>
    {
        [Theory]
        [InlineData(0, 100)]
        [InlineData(10, 100)]
        [InlineData(100, 100)]
        [InlineData(110, 100)]
        [InlineData(200, 100)]
        [InlineData(210, 100)]
        public void ShouldCompressAndDecompress(int length, int size)
        {
            // Arrange

            var sourceFilePath = tempFileProvider.GetTempFile();
            var targetFilePath = tempFileProvider.GetTempFile();
            var decompressedFilePath = tempFileProvider.GetTempFile();

            using (var writer = new FileStream(sourceFilePath, FileMode.Create, FileAccess.Write))
            {
                var data = new byte[length];
                new Random(100500).NextBytes(data);
                writer.Write(data);
            }

            // Act

            {
                var builder = new ContainerBuilder();
                builder.RegisterModule(new CompressorLibParallelModule(sourceFilePath, size, targetFilePath));
                using (var scope = builder.Build().BeginLifetimeScope())
                {
                    var compressor = scope.Resolve<IFileCompressor>();
                    compressor.Execute();
                }
            }

            {
                var builder = new ContainerBuilder();
                builder.RegisterModule(new CompressorLibParallelModule(targetFilePath, size, decompressedFilePath));
                using (var scope = builder.Build().BeginLifetimeScope())
                {
                    var compressor = scope.Resolve<IFileDecompressor>();
                    compressor.Execute();
                }
            }

            // Assert

            var original = new byte[new FileInfo(sourceFilePath).Length];
            using (var reader = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            {
                reader.Read(original, 0, original.Length);
            }

            var compressed = new byte[new FileInfo(targetFilePath).Length];
            using (var reader = new FileStream(targetFilePath, FileMode.Open, FileAccess.Read))
            {
                reader.Read(compressed, 0, compressed.Length);
            }

            var result = new byte[new FileInfo(decompressedFilePath).Length];
            using (var reader = new FileStream(decompressedFilePath, FileMode.Open, FileAccess.Read))
            {
                reader.Read(result, 0, result.Length);
            }

            Assert.Equal(original, result);
        }
        private readonly TempFileFixture tempFileProvider;
        public IntegrationTests(TempFileFixture tempFileProvider)
        {
            this.tempFileProvider = tempFileProvider;
        }
    }
}
