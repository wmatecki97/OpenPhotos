using Limilabs.FTP.Client;
using OpenPhotos.Contracts;
using OpenPhotos.Core.Interfaces;

namespace OpenPhotos.Core.FileSystem;

public class FtpFileSystem : IFileSystem, IDisposable
{
    private readonly Ftp _ftpConnection;
    private readonly IMessagePublisher _messagePublisher;

    public FtpFileSystem(IMessagePublisher messagePublisher)
    {
        _ftpConnection = new Ftp();
        _ftpConnection.Connect("ftpupload.net", 21);
        var login = Configuration.GetFtpLogin();
        var password = Configuration.GetFtpPassword();
        _ftpConnection.Login(login, password);
        CreateFtpFoldersStructure();
        _ftpConnection.ChangeFolder(Constants.OpenPhotosFolderPath);
        _messagePublisher = messagePublisher;
    }

    public void Dispose()
    {
        _ftpConnection.Dispose();
        GC.SuppressFinalize(this);
    }

    public byte[] GetFile(string name, bool fullQuality = false)
    {
        var folder = fullQuality ? Constants.FullQualityFolderName : Constants.ThumbnailsFolderName;
        name = $"{folder}/{name}";
        var file = _ftpConnection.Download(name);
        return file;
    }

    /// <summary>
    ///     Sends a message to save a file. This doesn't save the file itself due to possible errors.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileContent"></param>
    public void SaveFile(string fileName, byte[] fileContent)
    {
        var pyload = new FileSaveRequest
        {
            Content = fileContent,
            Name = fileName
        };

        _messagePublisher.PublishSaveFileMessage(pyload);
    }

    public List<string> GetAllFiles(string folderName = "./")
    {
        var list = _ftpConnection.List(folderName);
        var fileNames = list.Select(f => f.Name).ToList();
        return fileNames;
    }

    public void DeleteAllThumbnails()
    {
        _ftpConnection.DeleteFolderRecursively(Constants.ThumbnailsFolderName);
    }

    private void CreateFtpFoldersStructure()
    {
        _ftpConnection.CreateFolder(Constants.OpenPhotosFolderPath);
        _ftpConnection.CreateFolder(Constants.FullQualityFolderName);
        _ftpConnection.CreateFolder(Constants.ThumbnailsFolderName);
    }

    ~FtpFileSystem()
    {
        _ftpConnection.Dispose();
    }
}