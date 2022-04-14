using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpFundamentals.Task1.Listener
{
    internal class Program
    {
        private static readonly string _url = "http://localhost:8080/";

        static async Task Main(string[] args)
        {
            var listener = GetListener();
            Console.WriteLine("Start listener.");

            using (listener)
            {
                listener.Start();
                Console.WriteLine("Listening http://localhost:8080 ...");

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
            listener.Prefixes.Add(_url);

            return listener;
        }

        private static void SendResponse(HttpListenerContext context)
        {
            var name = GetNameFromRequest(context);

            HttpListenerResponse response;
            if (!string.IsNullOrWhiteSpace(name))
            {
                var responseDetails = GetMyName(name, context);
                response = responseDetails.response;
                var buffer = responseDetails.buffer;

                using (var output = response.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }
            }
            else
            {
                response = context.Response;
                response.StatusCode = 200;
                response.Close();
            }
        }

        private static string GetNameFromRequest(HttpListenerContext context)
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

        private static (HttpListenerResponse response, byte[] buffer) GetMyName(string name, HttpListenerContext context)
        {
            var response = context.Response;
            var responseText = $"Hello, {name}";
            var buffer = Encoding.UTF8.GetBytes(responseText);

            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/plain";

            return (response, buffer);
        }
    }
}
