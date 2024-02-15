using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClientBL.Controller
{
    public class FTPClientController
    {
        private FtpClient _client;
        private string _folder;

        public FTPClientController(string hostname, string username, string password)
        {
            #region Проверки входных данных
            if (string.IsNullOrWhiteSpace(hostname)) throw new ArgumentNullException(nameof(hostname));

            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            #endregion

            try
            {
                _client = new FtpClient(hostname, username, password);
                _client.Connect();
                _folder = "";
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка подключения");
            }
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }

        public void ChangeDirectory(string[] args)
        {
            if(args.Length <= 1)
            {
                return;
            }

            if (args[1].Equals(".."))
            {
                var dirs = _folder.Split('/');

                if (dirs.Length <= 1)
                {
                    _folder = string.Empty;
                }
                else
                {
                    _folder = string.Join("/", dirs.SkipLast(1));
                }
            }
            else if (args[1].Equals("~"))
            {
                _folder = $"C:\\Users\\{Environment.UserName}";
            }
            else
            {
                _folder += "/" + args[1];
            }

        }

        public void Delete(string[] args)
        {
            if(args.Length <= 1)
            {
                return;
            }

            string path = _folder + "/" + args[1];

            if (_client.FileExists(path))
            {
                _client.DeleteFile(path);
            }
        }

        public void DeleteDir(string[] args)
        {
            if (args.Length <= 1)
            {
                return;
            }

            var path = _folder + "/" + args[1];

            if (_client.DirectoryExists(path))
            {
                _client.DeleteDirectory(path);
            }
        }

        public void Download(string[] args)
        {
            if(args.Length <= 1)
            {
                return;
            }

            if (args.Length <= 2)
            {

                var path = _folder + "/" + args[1];

                if (_client.FileExists(args[1]))
                {
                    _client.DownloadFile(Environment.CurrentDirectory + "\\" + args[1], path, FtpLocalExists.Overwrite, FtpVerify.Retry);
                }
            }
            else
            {
                DownloadFiles(args);
            }
        }

        public void DownloadFiles(string[] args)
        {
            _client.DownloadFiles(Environment.CurrentDirectory, args.Skip(1).ToArray(), FtpLocalExists.Skip);
        }

        public void Upload(string[] args)
        {
            if (args.Length <= 1)
            {
                return;
            }
            
            if(args.Length <= 3)
            {
                var path = _folder + "/" + args[1];

                
                 _client.UploadFile(Environment.CurrentDirectory+"\\"+args[1], path, FtpRemoteExists.Overwrite, true ,FtpVerify.Retry);
                

            }
            else
            {
                UploadsFile(args);
            }
            

        }

        public void UploadsFile(string[] args)
        {
            _client.UploadFiles(args.Skip(1).ToArray(), Environment.CurrentDirectory+"/test.txt", FtpRemoteExists.Skip);
        }

        public void Move(string[] args)
        {
            if(args.Length <= 2)
            {
                return;
            }

            var path = _folder + "/" + args[1];

            if (_client.FileExists(path))
            {
                _client.MoveFile(path, args[2], FtpRemoteExists.Overwrite);
            }
        }
        
        public void CreateDirectory(string[] args)
        {
            if (args.Length <= 1)
            {
                return;
            }

            var path = _folder + args[1];

            _client.CreateDirectory(path, true);
        }

        public void GetList()
        {
            foreach(var item in _client.GetListing(_folder, FtpListOption.Recursive))
            {
                if(item.Type == FtpObjectType.File)
                {
                    Console.WriteLine($"{item.FullName}\t{_client.GetFileSize(item.FullName)}\t{_client.GetChmod(item.FullName)}" +
                        $"\t{_client.GetModifiedTime(item.FullName)}");
                }
                else if(item.Type == FtpObjectType.Directory)
                {
                    Console.WriteLine($"{item.FullName}\t{_client.GetModifiedTime(item.FullName)}");
                }
            }
        }

    }
}
