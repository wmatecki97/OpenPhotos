using Limilabs.FTP.Client;
using OpenPhotos.Contracts;

namespace OpenPhotos.FileSystem;

public class FtpFileSystemWriter : IDisposable
{
    private readonly Ftp _ftpConnection;

    public FtpFileSystemWriter()
    {
        _ftpConnection = new Ftp();
        _ftpConnection.Connect("ftpupload.net", 21);
        var login = Configuration.GetFtpLogin();
        var password = Configuration.GetFtpPassword();
        _ftpConnection.Login(login, password);
        _ftpConnection.CreateFolder(Constants.OpenPhotosFolderPath);
        _ftpConnection.ChangeFolder(Constants.OpenPhotosFolderPath);
        _ftpConnection.CreateFolder(Constants.FullQualityFolderName);
        _ftpConnection.CreateFolder(Constants.ThumbnailsFolderName);
    }

    public void Dispose()
    {
        _ftpConnection.Dispose();
        GC.SuppressFinalize(this);
    }

    ~FtpFileSystemWriter()
    {
        _ftpConnection.Dispose();
    }

    internal void SaveFile(string fileName, byte[] fileContent)
    {
        if (!_ftpConnection.FileExists(fileName)) _ftpConnection.Append(fileName, fileContent);
    }
}