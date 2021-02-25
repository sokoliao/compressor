using CompressorLib.Abstractions.Decompression;
using CompressorLib.Abstractions.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class ParallelFileDecompressor : IFileDecompressor
    {
        private readonly ParallelFileDecompressorOptions options;

        public ParallelFileDecompressor(ParallelFileDecompressorOptions options)
        {
            this.options = options;
        }
        public void Decompress()
        {
            var readers = new List<IFileReader>();

            var writer = new ConcurrentWriter(
                new FileWriter(options.TargetFilePath));

            var queueReader = new FileReader(options.SourceFilePath);
            readers.Add(queueReader);
            var queue = new ConcurrentGenerator<ChunkDecompressionInfo>(
                new DecompressionInfoGenerator(queueReader));

            var threads = new List<Thread>();

            for (var i = 0; i < options.ThreadCount; i++)
            {
                var reader = new FileReader(options.SourceFilePath);
                readers.Add(reader);
                var worker = new FileDecompressor(
                    queue,
                    reader,
                    new Decompressor(),
                    writer);
                threads.Add(new Thread(worker.Decompress));
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
                foreach (var reader in readers)
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
                    writer.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }

    public class ParallelFileDecompressorOptions
    {
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public int ThreadCount { get; set; }
    }
}
