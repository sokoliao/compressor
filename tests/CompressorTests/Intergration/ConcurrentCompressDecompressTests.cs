using Autofac;
using CompressorLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompressorTests.Intergration
{
    public class ConcurrentCompressDecompressTests : IClassFixture<TempFileFixture>
    {
        private readonly TempFileFixture tempFileProvider;

        public ConcurrentCompressDecompressTests(TempFileFixture tempFileProvider)
        {
            this.tempFileProvider = tempFileProvider;
        }

        [Theory]
        [InlineData(10, 100)]
        [InlineData(1000, 100)]
        [InlineData(1010, 100)]
        public void ShouldCompressDecompress(int length, int size)
        {
            var sourceFilePath = tempFileProvider.GetTempFile();
            var targetFilePath = tempFileProvider.GetTempFile();
            var decompressedFilePath = tempFileProvider.GetTempFile();

            using (var writer = new FileStream(sourceFilePath, FileMode.Create, FileAccess.Write))
            {
                var data = new byte[length];
                new Random(100500).NextBytes(data);
                writer.Write(data);
            }

            {
                var builder = new ContainerBuilder();
                //builder.RegisterModule(new CompressorLibParallelModule());
            }

            //// Arrange 

            //var sourceFilePath = tempFileProvider.GetTempFile();
            //using (var writer = new FileStream(sourceFilePath, FileMode.Create, FileAccess.Write))
            //{
            //    var data = new byte[length];
            //    new Random(100500).NextBytes(data);
            //    writer.Write(data);
            //}

            //var targetFilePath = tempFileProvider.GetTempFile();

            //var compressor = new ParallelFileCompressor(
            //    new ParallelFileCompressorOptions 
            //    {
            //        ChunkSize = size,
            //        SourceFilePath = sourceFilePath,
            //        TargetFilePath = targetFilePath,
            //        ThreadCount = 4
            //    });

            //var decompressedFilePath = tempFileProvider.GetTempFile();

            //var decompressor = new ParallelFileDecompressor(
            //    new ParallelFileDecompressorOptions
            //    {
            //        SourceFilePath = targetFilePath,
            //        TargetFilePath = decompressedFilePath,
            //        ThreadCount = 4
            //    });

            //// Act

            //compressor.Execute();

            //decompressor.Execute();

            //// Assert

            //var original = new byte[new FileInfo(sourceFilePath).Length];
            //using (var reader = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            //{
            //    reader.Read(original, 0, original.Length);
            //}

            //var compressed = new byte[new FileInfo(targetFilePath).Length];
            //using (var reader = new FileStream(targetFilePath, FileMode.Open, FileAccess.Read))
            //{
            //    reader.Read(compressed, 0, compressed.Length);
            //}

            //var result = new byte[new FileInfo(decompressedFilePath).Length];
            //using (var reader = new FileStream(decompressedFilePath, FileMode.Open, FileAccess.Read))
            //{
            //    reader.Read(result, 0, result.Length);
            //}

            //Assert.Equal(original, result);
        }
    }
}
