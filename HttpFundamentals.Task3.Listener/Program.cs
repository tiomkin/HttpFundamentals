using System;
using System.Net;
using System.Threading.Tasks;

namespace HttpFundamentals.Task3_Task4.Listener
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var listener = GetListener();
            Console.WriteLine("Start listener.");

            using (listener)
            {
                listener.Start();
                Console.WriteLine("Listening http://localhost:8888 ...");

                while (true)
                {
                    var context = await listener.GetContextAsync();

                    var request = context.Request;
                    Console.WriteLine($"Requested url: {request.Url}");

                    SendResponse(context);
                    Console.WriteLine("Response is sent.");
                }
            }
        }

        private static HttpListener GetListener()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8888/MyNameByHeader/");
            listener.Prefixes.Add("http://localhost:8888/MyNameByCookies/");

            return listener;
        }

        private static void SendResponse(HttpListenerContext context)
        {
            var name = GetMethodNameFromRequest(context);
            var response = context.Response;

            switch (name)
            {
                case "MyNameByHeader":
                    GetMyNameByHeader(response);
                    break;
                case "MyNameByCookies":
                    MyNameByCookies(response);
                    break;
                default:
                    response.StatusCode = 200;
                    break;
            }

            response.StatusCode = 200;
            response.Close();
        }

        private static string GetMethodNameFromRequest(HttpListenerContext context)
        {
            var segments = context.Request.Url?.Segments;

            string name = string.Empty;
            if (segments.Length >= 2)
            {
                name = segments[1];
                if (name.EndsWith('/'))
                {
                    name = name.Remove(name.Length - 1);
                }
            }

            return name;
        }

        private static void GetMyNameByHeader(HttpListenerResponse response)
        {
            response.AddHeader("X-MyName", "Test Name");
        }

        private static void MyNameByCookies(HttpListenerResponse response)
        {
            response.SetCookie(new Cookie("Name", "Test Name"));
        }
    }
}
