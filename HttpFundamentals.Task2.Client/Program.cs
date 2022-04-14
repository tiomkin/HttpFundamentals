using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpFundamentals.Task2.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);

            Console.WriteLine("The following requests are accepted:");
            Console.WriteLine("http://localhost:8888/Information/");
            Console.WriteLine("http://localhost:8888/Success/");
            Console.WriteLine("http://localhost:8888/Redirection/");
            Console.WriteLine("http://localhost:8888/ClientError/");
            Console.WriteLine("http://localhost:8888/ServerError/");
            Console.WriteLine("Enter your request: ");

            string command;
            while ((command = Console.ReadLine()) != "exit")
            {
                try
                {
                    var response = await client.GetStringAsync(command);
                    Console.Write("Response status code:");
                    Console.WriteLine(response);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong. Exception: {e.Message}");
                }
            }
        }
    }
}
