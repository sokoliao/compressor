using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorLib.Abstractions.Compressions
{
    public struct ChunkCompressionInfo
    {
        public ChunkInfo source;
        public ChunkCompressionInfo(ChunkInfo source)
        {
            this.source = source;
        }
    }
}
