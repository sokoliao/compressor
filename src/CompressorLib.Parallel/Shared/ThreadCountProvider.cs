using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Parallel.Shared
{
    public class ThreadCountProvider : IThreadCountProvider
    {
        public int GetCount() => Math.Max(Environment.ProcessorCount - 1, 1);
    }
    public interface IThreadCountProvider
    {
        int GetCount();
    }
}
