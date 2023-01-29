using Limilabs.FTP.Client;

namespace OpenPhotos.Core.Features.FTP
{
    public class FtpFileSystem
    {
        private readonly Ftp ftpConnection;

        public FtpFileSystem()
        {
            ftpConnection= new Ftp();
            ftpConnection.Connect("192.168.0.1", 21);
            var login = Configuration.GetFtpLogin();
            var password = Configuration.GetFtpPassword();
            ftpConnection.Login(login, password);
            ftpConnection.ChangeFolder("G/OpenPhotos");
        }

        ~FtpFileSystem()
        {
            ftpConnection.Dispose();
        }

        public byte[] GetFile(string name)
        {
            var file = ftpConnection.Download(name);
            return file;
        }

        public void SaveFile(string fileName, byte[] fileContent) 
        {
            var date = DateTime.Now;
            string name = $"{date.Year}-{date.Month}-{date.Day}-{fileName}";
            ftpConnection.Append(name, fileContent);
        }

        public List<string> GetAllFiles()
        {
            var list = ftpConnection.List();
            var fileNames = list.Select(f => f.Name).ToList();
            return fileNames;
        }
    }
}
