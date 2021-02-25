using CompressorLib.Abstractions.Compressions;
using CompressorLib.Abstractions.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class ParallelFileCompressor : IFileCompressor
    {
        private readonly ParallelFileCompressorOptions options;

        public ParallelFileCompressor(ParallelFileCompressorOptions options)
        {
            this.options = options;
        }
        public void Compress()
        {
            var compressionInputs = new List<IFileReader>();
            var compressionOutput = new ConcurrentWriter(
                new FileWriter(options.TargetFilePath));
            var queue = new ConcurrentGenerator<ChunkCompressionInfo>(
                new CompressionInfoGenerator(
                    new CompressorInfoGeenratorOptions
                    {
                        Path = options.SourceFilePath,
                        Size = options.ChunkSize
                    },
                    new FileService()));
            var threads = new List<Thread>();

            for (var i = 0; i < options.ThreadCount; i++)
            {
                var compressionInput = new FileReader(options.SourceFilePath);
                compressionInputs.Add(compressionInput);
                var worker = new FileCompressor(
                    new FileCompressorOptions { Size = options.ChunkSize },
                    queue,
                    compressionInput,
                    new Compressor(),
                    compressionOutput);
                threads.Add(new Thread(worker.Compress));
            }

            try
            {
                foreach (var thread in threads)
                {
                    thread.Start();
                }

                foreach (var thread in threads)
                {
                    thread.Join();
                }
            }
            finally
            {
                foreach (var reader in compressionInputs)
                {
                    try
                    {
                        reader.Dispose();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                try
                {
                    compressionOutput.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }            
        }
    }

    public class ParallelFileCompressorOptions
    {
        public int ChunkSize { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public int ThreadCount { get; set; }
    }
}
