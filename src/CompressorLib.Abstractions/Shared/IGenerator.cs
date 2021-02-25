using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Abstractions.Shared
{
    public interface IGenerator<T>
    {
        bool TryDequeue(out T item);
    }
}
