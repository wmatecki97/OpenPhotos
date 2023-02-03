namespace OpenPhotos.Core.Interfaces;

public interface IFileSystem
{
    void Dispose();
    List<string> GetAllFiles();
    byte[] GetFile(string name);
    void SaveFile(string fileName, byte[] fileContent);
}