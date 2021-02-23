namespace CompressorLib
{
  public class FileChunkSlicer : IFileChunkSlicer
  {
    private readonly IFileService fileService;

    public FileChunkSlicer(IFileService fileService)
    {
      this.fileService = fileService;
    }
    public IChunkSlicer Slicer(string filePath, int size) => 
      new ChunkSlicer(size, fileService.GetSize(filePath));
  }
  public interface IFileChunkSlicer
  {
    IChunkSlicer Slicer(string filePath, int size);
  }
}