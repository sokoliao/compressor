using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompressorLib.Parallel.Shared
{
    public class ConcurrentGenerator<T> : IGenerator<T>
    {
        private readonly IGenerator<T> decoratee;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public ConcurrentGenerator(IGenerator<T> decoratee)
        {
            this.decoratee = decoratee;
        }
        public bool TryDequeue(out T item)
        {
            semaphore.Wait();
            try
            {
                return decoratee.TryDequeue(out item);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
