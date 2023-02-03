using Limilabs.FTP.Client;
using OpenPhotos.Contracts;
using OpenPhotos.Core.Interfaces;

namespace OpenPhotos.Core.FileSystem;

public class FtpFileSystem : IFileSystem, IDisposable
{
    private readonly Ftp ftpConnection;
    private readonly IMessagePublisher messagePublisher;

    public FtpFileSystem(IMessagePublisher messagePublisher)
    {
        ftpConnection = new Ftp();
        ftpConnection.Connect("ftpupload.net", 21);
        var login = Configuration.GetFtpLogin();
        var password = Configuration.GetFtpPassword();
        ftpConnection.Login(login, password);
        ftpConnection.ChangeFolder("OpenPhotos");
        this.messagePublisher = messagePublisher;
    }

    public void Dispose()
    {
        ftpConnection.Dispose();
        GC.SuppressFinalize(this);
    }

    public byte[] GetFile(string name)
    {
        var file = ftpConnection.Download(name);
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

        messagePublisher.PublishSaveFileMessage(pyload);
    }

    public List<string> GetAllFiles()
    {
        var list = ftpConnection.List();
        var fileNames = list.Select(f => f.Name).ToList();
        return fileNames;
    }

    ~FtpFileSystem()
    {
        ftpConnection.Dispose();
    }
}