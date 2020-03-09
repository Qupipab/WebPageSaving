using System;
using System.IO;
using System.Net.Sockets;

namespace WebPageSaving
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите команду, путь к странице, название файла.");

            string[] commandsArr = Console.ReadLine().Split(new char[] { ' ' });
            string command = commandsArr[0];
            if (commandsArr[1].Contains("http://")) commandsArr[1] = commandsArr[1].Substring(7);
            string siteName = commandsArr[1].Split('/')[0];
            string sitePath = commandsArr[1];

            if (commandsArr[1].IndexOf("/") != -1)
            {
                sitePath = commandsArr[1].Substring(commandsArr[1].IndexOf('/'));
            }

            string filePath = "..\\..\\..\\SavedFiles\\" + commandsArr[2];

            if (command == "pagesaver")
            {
                string msg = $"GET /{sitePath} HTTP/1.0\r\nHost: {siteName}\r\n\r\n";
                try
                {
                    var port = 80;
                    var client = new TcpClient(siteName, port);
                    var data = System.Text.Encoding.ASCII.GetBytes(msg);
                    NetworkStream stream = client.GetStream();

                    stream.Write(data, 0, data.Length);
                    stream.Flush();

                    var responseData = new byte[4096];
                    int bytesRead = stream.Read(responseData, 0, responseData.Length);
                    var responseMessage = System.Text.Encoding.ASCII.GetString(responseData, 0, bytesRead);
                    stream.Close();
                    client.Close();

                    if (responseMessage.IndexOf("<html>", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        int searchIndex = responseMessage.IndexOf("<html>", StringComparison.OrdinalIgnoreCase);
                        responseMessage = responseMessage.Substring(searchIndex);
                    }
                    else if (responseMessage.IndexOf("<header>", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        int searchIndex = responseMessage.IndexOf("<header>", StringComparison.OrdinalIgnoreCase);
                        responseMessage = responseMessage.Substring(searchIndex);
                    }

                    using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(responseMessage);
                    }
                    Console.WriteLine("Запись выполнена");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Excep {0}", e);
                }
            }
        }
    }
}
