﻿using Limilabs.FTP.Client;
using OpenPhotos.Core.Interfaces;

namespace OpenPhotos.Core.FileSystem
{
    public class FtpFileSystem : IFileSystem, IDisposable
    {
        private readonly Ftp ftpConnection;

        public FtpFileSystem()
        {
            ftpConnection = new Ftp();
            ftpConnection.Connect("ftpupload.net", 21);
            var login = Configuration.GetFtpLogin();
            var password = Configuration.GetFtpPassword();
            ftpConnection.Login(login, password);
            ftpConnection.ChangeFolder("OpenPhotos");
        }

        public void Dispose()
        {
            ftpConnection.Dispose();
            GC.SuppressFinalize(this);
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
            ftpConnection.Append(fileName, fileContent);
        }

        public List<string> GetAllFiles()
        {
            var list = ftpConnection.List();
            var fileNames = list.Select(f => f.Name).ToList();
            return fileNames;
        }        
    }
}
