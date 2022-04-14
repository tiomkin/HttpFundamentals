using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpFundamentals.Task3_Task4.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);

            Console.WriteLine("The following requests are accepted:");
            Console.WriteLine("http://localhost:8888/MyNameByHeader/");
            Console.WriteLine("http://localhost:8888/MyNameByCookies/");
            Console.WriteLine("Enter your request: ");

            string command;
            while ((command = Console.ReadLine()) != "exit")
            {
                try
                {
                    var response = await client.GetAsync(command);
                    var headers = response.Headers;
                    Console.WriteLine(headers.TryGetValues("X-MyName", out var values)
                        ? $"The values for X-MyName header is: {values.ToList()[0]}"
                        : "There is no header with name X-MyName.");
                    Console.WriteLine(headers.TryGetValues("Set-Cookie", out var cookies)
                    ? $"Cookie value: {GetCookieValueByName("Name", cookies)}"
                    : "There are no cookies in response");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong. Exception: {e.Message}");
                }

                Console.WriteLine("Enter your request: ");
            }
        }

        private static string GetCookieValueByName(string name, IEnumerable<string> cookies)
        {
            foreach (var cookie in cookies)
            {
                if (cookie.StartsWith($"{name}="))
                {
                    return name.Substring(name.IndexOf('=') + 1);
                }
            }

            return $"There is no cookie with name: {name}";
        }
    }
}
