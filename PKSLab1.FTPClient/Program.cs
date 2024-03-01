

using FluentFTP;
using FTPClientBL.Controller;


FTPClientController client = new FTPClientController(LogIn());


if(client != null)
{
    while (true)
    {
        try
        {
            Console.Write("Ftp >>>  ");
            var command = Console.ReadLine().Split(" ");

            switch (command[0])
            {
                case "download":
                    client.Download(command);
                    break;
                case "upload":
                    client.Upload(command);
                    break;
                case "ls":
                    client.GetList();
                    break;
                case "cd":
                    client.ChangeDirectory(command);
                    break;
                case "delete":
                    client.Delete(command);
                    break;
                case "mv":
                    client.Move(command);
                    break;
                case "mkdir":
                    client.CreateDirectory(command);
                    break;
                case "rmdir":
                    client.DeleteDir(command);
                    break;
                case "disconnect":
                    client.Disconnect();
                    client = new FTPClientController(LogIn());
                    break;
                case "help":
                    Console.WriteLine("download\nupload\nls\ncd\ndelete\nmv\nmkdir\nrmdir\ndisconnect");
                    break;
                default:
                    Console.WriteLine("Несуществующая команда!");
                    break;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("Неверный синтаксис команды!Попробуйте иначе!");
            Console.ReadLine();
            continue;
        }
    }
}







FtpClient LogIn()
{
    string[] connectionString;
    var client = new FtpClient();
    while (true)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Введите hostname, username, password:");
            Console.Write(">");
            connectionString = Console.ReadLine().Split(" ");

            if (connectionString.Length == 3)
            {
                client = new FtpClient(connectionString[0], connectionString[1], connectionString[2]);
                client.Connect();
                break;
            }
            else
            {
                Console.WriteLine("Неверный формат!");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("Ошибка авторизации. Проверьте корректность данных!");
            Console.ReadLine();
            continue;
        }
    }



    return client;
}