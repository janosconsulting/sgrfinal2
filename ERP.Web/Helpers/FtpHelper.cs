using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ERP.Web.Helpers
{
    public class FtpSettings
    {
        public int Port { get; set; }
        public string Server { get; set; }
        public string RemoteFolderPath { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string dato { get; set; }

    }

    public class FtpFile
    {
        public string FileName;
        public DateTime TimeStamp;
    }


    public class FtpHelper
    {

        public readonly FtpSettings Settings;

        public FtpHelper(FtpSettings settings)
        {
            this.Settings = settings;
        }


        private FtpWebRequest CreateRequest(string remotePath)
        {
            var builder = new UriBuilder();
            builder.Scheme = "ftp";
            builder.Port = this.Settings.Port;
            builder.Host = this.Settings.Server;
            if (!string.IsNullOrEmpty(remotePath))
            {
                builder.Path = remotePath;
            }
            var request = (FtpWebRequest)FtpWebRequest.Create(builder.Uri);
            if (!string.IsNullOrEmpty(this.Settings.User) && !string.IsNullOrEmpty(this.Settings.Password))
            {
                request.Credentials = new NetworkCredential(this.Settings.User, this.Settings.Password);
            }
            request.KeepAlive = true;
            return request;
        }

        public List<FtpFile> GetRemoteFiles()
        {
            var ftpClient = CreateRequest(this.Settings.RemoteFolderPath);
            ftpClient.Method = WebRequestMethods.Ftp.ListDirectory;
            var files = new List<FtpFile>();
            using (var response = (FtpWebResponse)ftpClient.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                while (!reader.EndOfStream)
                {
                    string fileName = reader.ReadLine();
                    files.Add(new FtpFile
                    {
                        FileName = fileName
                    });
                }
            }
            foreach (var f in files)
            {
                ftpClient = CreateRequest(GetRemoteFilePath(f.FileName));
                ftpClient.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                using (var response = (FtpWebResponse)ftpClient.GetResponse())
                {
                    f.TimeStamp = response.LastModified;
                }
            }
            files.Sort((f1, f2) => f1.TimeStamp.CompareTo(f2.TimeStamp));
            return files;
        }
        private string GetRemoteFilePath(string fileName)
        {
            if (string.IsNullOrEmpty(this.Settings.RemoteFolderPath))
            {
                return "/" + fileName;
            }
            else
            {
                return this.Settings.RemoteFolderPath.TrimEnd('/') + "/" + fileName;
            }
        }
    }
}