using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpFundamentals.Task1.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            Console.WriteLine("Requests with the following base uri are accepted: http://localhost:8080/");
            Console.WriteLine("To add your name to response use the following request with your name instead of \"YourName\": http://localhost:8080/YourName/");
            Console.WriteLine("Enter your request: ");

            string command;
            while ((command = Console.ReadLine()) != "exit")
            {
                try
                {
                    var response = await client.GetStringAsync(command);
                    Console.WriteLine("Response:");
                    Console.WriteLine(!string.IsNullOrWhiteSpace(response) ? response : "There is no name in a response");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong. Exception: {e.Message}");
                }
            }
        }
    }
}
