using Autofac;
using CompressorLib.Abstractions.Compression;
using CompressorLib.Abstractions.Compressions;
using CompressorLib.Abstractions.Decompression;
using CompressorLib.Abstractions.IO;
using CompressorLib.Abstractions.Shared;
using CompressorLib.Parallel.Compression;
using CompressorLib.Parallel.Decompression;
using CompressorLib.Parallel.IO;
using CompressorLib.Parallel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompressorLib.Parallel.Creation
{
    public class CompressorLibParallelModule : Module
    {
        private readonly string sourceFilePath;
        private readonly int chunkSize;
        private readonly string targetFilePath;

        public CompressorLibParallelModule(string sourceFilePath, int chunkSize, string targetFilePath)
        {
            this.sourceFilePath = sourceFilePath;
            this.chunkSize = chunkSize;
            this.targetFilePath = targetFilePath;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => new FileReader(sourceFilePath)).As<IFileReader>();
            builder.Register(ctx => new ConcurrentWriter(new FileWriter(targetFilePath)))
                .As<IFileWriter>()
                .InstancePerLifetimeScope();
            builder.RegisterType<FileService>().As<IFileService>();
            builder.RegisterType<ThreadCountProvider>().As<IThreadCountProvider>();

            builder.RegisterType<Compressor>().As<ICompressor>();
            builder.Register(_ => 
                new CompressorInfoGeenratorOptions
                {
                    Path = sourceFilePath,
                    Size = chunkSize
                }).AsSelf();
            builder.RegisterType<CompressionInfoGenerator>().AsSelf();
            builder.Register(ctx => 
                    new ConcurrentGenerator<ChunkCompressionInfo>(
                        ctx.Resolve<CompressionInfoGenerator>()))
                .As<IGenerator<ChunkCompressionInfo>>()
                .InstancePerLifetimeScope();
            builder.Register(ctx => new FileCompressorOptions { Size = chunkSize }).AsSelf();
            builder.RegisterType<FileCompressor>().AsSelf();
            builder.RegisterType<ParallelFileCompressor>().As<IFileCompressor>();

            builder.RegisterType<Decompressor>().As<IDecompressor>();
            builder.RegisterType<DecompressionInfoGenerator>().AsSelf();
            builder.Register(ctx =>
                    new ConcurrentGenerator<ChunkDecompressionInfo>(
                        ctx.Resolve<DecompressionInfoGenerator>()))
                .As<IGenerator<ChunkDecompressionInfo>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<FileDecompressor>().AsSelf();
            builder.RegisterType<ParallelFileDecompressor>().As<IFileDecompressor>();
        }
    }
}
