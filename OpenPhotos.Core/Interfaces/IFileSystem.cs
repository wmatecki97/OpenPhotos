namespace OpenPhotos.Core.Interfaces;

public interface IFileSystem
{
    void Dispose();
    List<string> GetAllFiles();
    byte[] GetFile(string name, bool fullQuality = false);
    void SaveFile(string fileName, byte[] fileContent);
}