using CompressorLib.Abstractions.Compressions;
using CompressorLib.Abstractions.IO;
using CompressorLib.Abstractions.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorLib
{
    public class CompressionInfoGenerator : IGenerator<ChunkCompressionInfo>
    {
        private long position = 0;
        private int size;
        private Lazy<long> length;

        public CompressionInfoGenerator(
            CompressorInfoGeenratorOptions options,
            IFileService fileService)
        {
            size = options.Size;
            length = new Lazy<long>(() => fileService.GetSize(options.Path));
        }

        public bool TryDequeue(out ChunkCompressionInfo info)
        {
            var offset = position * size;
            if (offset >= length.Value)
            {
                info = default;
                return false;
            }
            var chunkEnd = offset + size;
            if (chunkEnd > length.Value)
            {
                chunkEnd = length.Value;
            }
            info = new ChunkCompressionInfo(new ChunkInfo(offset, (int)(chunkEnd - offset)));
            position++;
            return true;
        }
    }
    public class CompressorInfoGeenratorOptions
    {
        public int Size { get; set; }
        public string Path { get; set; }
    }
}
