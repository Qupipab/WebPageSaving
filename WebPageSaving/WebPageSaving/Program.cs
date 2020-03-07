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
            string sitePath = commandsArr[1];
            string filePath = "..\\..\\..\\SavedFiles\\" + commandsArr[2];

            if (command == "pagesaver")
            {
                string msg = "GET / HTTP/1.0\nHost: " + sitePath + "\n\n";
                try
                {
                    var port = 80;
                    var serverAddr = sitePath;
                    var client = new TcpClient(serverAddr, port);
                    var data = System.Text.Encoding.ASCII.GetBytes(msg);
                    NetworkStream stream = client.GetStream();

                    stream.Write(data, 0, data.Length);
                    stream.Flush();

                    var responseData = new byte[1024];
                    int bytesRead = stream.Read(responseData, 0, responseData.Length);
                    var responseMessage = System.Text.Encoding.ASCII.GetString(responseData, 0, bytesRead);
                    stream.Close();
                    client.Close();

                    responseMessage = responseMessage.Substring(responseMessage.IndexOf("<!Doctype"));
                    using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(responseMessage);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Excep {0}", e);
                }
                Console.WriteLine("Запись выполнена");
            }
        }
    }
}
