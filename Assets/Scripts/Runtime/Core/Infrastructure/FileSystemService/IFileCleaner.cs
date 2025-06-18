namespace Runtime.Core.Infrastructure.FileSystemService
{
    public interface IFileCleaner
    {
        bool TryCleanFolder(string folderPath);
        bool DestroyFolder(string filePath);
    }
}