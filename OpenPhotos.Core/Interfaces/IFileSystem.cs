namespace OpenPhotos.Core.Interfaces;

public interface IFileSystem
{
    void Dispose();
    List<string> GetAllFiles(string folderName = "./");
    byte[] GetFile(string name, bool fullQuality = false);
    void SaveFile(string fileName, byte[] fileContent);
    void DeleteAllThumbnails();
}