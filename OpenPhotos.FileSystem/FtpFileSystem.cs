using Limilabs.FTP.Client;
using OpenPhotos.Contracts;

namespace OpenPhotos.FileSystem
{
    public class FtpFileSystemWriter : IDisposable
    {
        private readonly Ftp ftpConnection;

        public FtpFileSystemWriter()
        {
            ftpConnection = new Ftp();
            ftpConnection.Connect("ftpupload.net", 21);
            var login = Configuration.GetFtpLogin();
            var password = Configuration.GetFtpPassword();
            ftpConnection.Login(login, password);
            ftpConnection.CreateFolder("OpenPhotos");
            ftpConnection.ChangeFolder("OpenPhotos");
            ftpConnection.CreateFolder(Constants.FullQualityFolderName);
            ftpConnection.CreateFolder(Constants.ThumbnailsFolderName);
        }

        public void Dispose()
        {
            ftpConnection.Dispose();
            GC.SuppressFinalize(this);
        }

        ~FtpFileSystemWriter()
        {
            ftpConnection.Dispose();
        }

        internal void SaveFile(string fileName, byte[] fileContent)
        {
            if(!ftpConnection.FileExists(fileName))
            {
                ftpConnection.Append(fileName, fileContent);
            }
        }
    }
}
