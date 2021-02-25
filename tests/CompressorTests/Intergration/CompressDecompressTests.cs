using Autofac;
using CompressorLib;
using CompressorLib.Abstractions.Compression;
using CompressorLib.Abstractions.Decompression;
using CompressorLib.Abstractions.Shared;
using CompressorLib.Creation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests.Intergration
{
    public class CompressDecompressTests : IClassFixture<TempFileFixture>
    {
        private readonly TempFileFixture tempFileProvider;

        public CompressDecompressTests(TempFileFixture tempFileProvider)
        {
            this.tempFileProvider = tempFileProvider;
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(10, 100)]
        [InlineData(100, 100)]
        [InlineData(110, 100)]
        [InlineData(200, 100)]
        [InlineData(210, 100)]
        public void ShouldCompressAndDecompress(int inputFileLength, int chunkSize)
        {
            // Arrange

            var originalFilePath = tempFileProvider.GetTempFile();
            using (var writer = new FileStream(originalFilePath, FileMode.Create, FileAccess.Write))
            {
                var data = new byte[inputFileLength];
                new Random(100500).NextBytes(data);
                writer.Write(data);
            }

            var compressedFilePath = tempFileProvider.GetTempFile();

            {
                var builder = new ContainerBuilder();
                builder.RegisterModule(new CompressorLibModule(
                    originalFilePath,
                    chunkSize,
                    compressedFilePath));
                var container = builder.Build();
                using (var scope = container.BeginLifetimeScope())
                {
                    var compressor = scope.Resolve<IFileCompressor>();
                    compressor.Execute();
                }
            }

            //var compressionInput = new FileReader(originalFilePath);
            //var compressionOutput = new FileWriter(compressedFilePath);
            //var compressor = new FileCompressor(
            //    new FileCompressorOptions { Size = chunkSize },
            //    new CompressionInfoGenerator(
            //        new CompressorInfoGeenratorOptions { Path = originalFilePath,  Size = chunkSize },
            //        new FileService()),
            //    compressionInput,
            //    new Compressor(),
            //    compressionOutput);

            var decompressedFilePath = tempFileProvider.GetTempFile();
            //var decompressionInput = new FileReader(compressedFilePath);
            //var decompressionOutput = new FileWriter(decompressedFilePath);
            //var decompressor = new FileDecompressor(
            //    new DecompressionInfoGenerator(decompressionInput),
            //    decompressionInput,
            //    new Decompressor(),
            //    decompressionOutput);

            {
                var builder = new ContainerBuilder();
                builder.RegisterModule(new CompressorLibModule(
                    compressedFilePath,
                    chunkSize,
                    decompressedFilePath));
                var container = builder.Build();
                using (var scope = container.BeginLifetimeScope())
                {
                    var compressor = scope.Resolve<IFileDecompressor>();
                    compressor.Execute();
                }
            }



            // Act

            //compressor.Execute();
            //compressionInput.Dispose();
            //compressionOutput.Dispose();

            //decompressor.Execute();
            //decompressionInput.Dispose();
            //decompressionOutput.Dispose();

            // Assert

            string original;
            using (var reader = new StreamReader(originalFilePath))
            {
                original = reader.ReadToEnd();
            }

            string result;
            using (var reader = new StreamReader(decompressedFilePath))
            {
                result = reader.ReadToEnd();
            }

            Assert.Equal(original, result);
        }
    }
}
