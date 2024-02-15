

using FluentFTP;
using FTPClientBL.Controller;


FTPClientController client = LogIn();


if(client != null)
{
    while (true)
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
                client = LogIn();
                break;
            default: Console.WriteLine("Несуществующая команда!"); 
                break;
        }

    }
}







FTPClientController LogIn()
{
    string[] connectionString;
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Введите hostname, username, password:");
        Console.Write(">");
        connectionString = Console.ReadLine().Split(" ");

        if(connectionString.Length == 3)
        {
            break;
        }
        else
        {
            Console.WriteLine("Неверный формат!");
        }
    }



    return new FTPClientController(connectionString[0], connectionString[1], connectionString[2]);
}