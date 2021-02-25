using CompressorLib.Abstractions.Compression;
using CompressorLib.Abstractions.Compressions;
using CompressorLib.Abstractions.IO;
using CompressorLib.Abstractions.Shared;
using CompressorLib.Parallel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompressorLib.Parallel.Compression
{
    public class ParallelFileCompressor : IWorker, IFileCompressor
    {
        private readonly Lazy<int> count;
        private readonly Func<FileCompressor> workerFactory;

        public ParallelFileCompressor(
            IThreadCountProvider threadCountProvider,
            Func<FileCompressor> workerFactory)
        {
            count = new Lazy<int>(() => threadCountProvider.GetCount());
            this.workerFactory = workerFactory;
        }

        public void Execute()
        {
            var threads = new List<Thread>();
            for (var i = 0; i < count.Value; i++)
            {
                threads.Add(new Thread(workerFactory().Execute));
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}
