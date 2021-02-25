using System;
using Xunit;
using CompressorLib;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using CompressorTests.Mocks;
using CompressorLib.Abstractions.IO;

namespace CompressorTests
{
    public class FileCompressorTests
    {
        [Fact]
        public void ShouldCompress()
        {
            // Arrange

            var fileService = new Mock<IFileService>();
            fileService.Setup(m => m.GetSize(It.IsAny<string>())).Returns(12);

            var file = new byte[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var reader = new InmemoryReader(file);

            var compressor = new Mocks.PassThroughCompressor();

            var output = new byte[44];
            var writer = new InmemoryWriter(output);

            var fileCompressor = new FileCompressor(
                new FileCompressorOptions { Size = 10 },
                new CompressionInfoGenerator(
                    new CompressorInfoGeenratorOptions { Path = @"C:\file.txt", Size = 10 },
                    fileService.Object),
                reader,
                compressor,
                writer);

            // Act

            fileCompressor.Compress();

            // Assert

            Assert.Equal(
                new byte[16 + 10 + 16 + 2]
                {
                    /* compressedSize */ 10, 0, 0, 0, /*  source.offset */ 0, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 10, 0, 0, 0,
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                    /* compressedSize */ 2, 0, 0, 0, /* source.offset */ 10, 0, 0, 0, 0, 0, 0, 0, /* source.size */ 2, 0, 0, 0,
                    11, 12
                }, output);
        }
    }
}
