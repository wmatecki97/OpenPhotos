namespace OpenPhotos.FileSystem;

public interface IFileSystem
{
    void Dispose();
    List<string> GetAllFiles();
    byte[] GetFile(string name);
}