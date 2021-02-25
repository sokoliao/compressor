using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Abstractions.IO
{
    public interface IFileService
    {
        bool Exists(string filePath);
        long GetSize(string filePath);
    }
}
