using Autofac;
using CompressorLib.Abstractions.Compression;
using CompressorLib.Abstractions.Compressions;
using CompressorLib.Abstractions.Decompression;
using CompressorLib.Abstractions.IO;
using CompressorLib.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Creation
{
    public class CompressorLibModule : Module
    {
        private readonly string sourceFilePath;
        private readonly int chunkSize;
        private readonly string targetFilePath;

        public CompressorLibModule(string sourceFilePath, int chunkSize, string targetFilePath)
        {
            this.sourceFilePath = sourceFilePath;
            this.chunkSize = chunkSize;
            this.targetFilePath = targetFilePath;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => new FileReader(sourceFilePath)).As<IFileReader>();
            builder.Register(ctx => new FileWriter(targetFilePath)).As<IFileWriter>();
            builder.RegisterType<FileService>().As<IFileService>();

            builder.RegisterType<Compressor>().As<ICompressor>();
            builder.Register(ctx =>
                    new CompressionInfoGenerator(
                        new CompressorInfoGeenratorOptions
                        {
                            Path = sourceFilePath,
                            Size = chunkSize
                        },
                        ctx.Resolve<IFileService>()))
                .As<IGenerator<ChunkCompressionInfo>>();
            builder.Register(ctx => new FileCompressorOptions { Size = chunkSize }).AsSelf();
            builder.RegisterType<FileCompressor>().As<IFileCompressor>();

            builder.RegisterType<Decompressor>().As<IDecompressor>();
            builder.RegisterType<DecompressionInfoGenerator>().As<IGenerator<ChunkDecompressionInfo>>();
            builder.RegisterType<FileDecompressor>().As<IFileDecompressor>();
        }
    }
}
