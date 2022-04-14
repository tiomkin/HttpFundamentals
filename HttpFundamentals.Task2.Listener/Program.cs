using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpFundamentals.Task2.Listener
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
            listener.Prefixes.Add("http://localhost:8888/Information/");
            listener.Prefixes.Add("http://localhost:8888/Success/");
            listener.Prefixes.Add("http://localhost:8888/Redirection/");
            listener.Prefixes.Add("http://localhost:8888/ClientError/");
            listener.Prefixes.Add("http://localhost:8888/ServerError/");

            return listener;
        }

        private static void SendResponse(HttpListenerContext context)
        {
            var name = GetStatusCodeNameFromRequest(context);
            var response = context.Response;

            switch (name)
            {
                case "Information":
                    SetInformationResponse(response);
                    break;
                case "Success":
                    SetSuccessResponse(response);
                    break;
                case "Redirection":
                    SetRedirectionResponse(response);
                    break;
                case "ClientError":
                    SetClientErrorResponse(response);
                    break;
                case "ServerError":
                    SetServerErrorResponse(response);
                    break;
                default:
                    SetSuccessResponse(response);
                    break;
            }

            var buffer = Encoding.UTF8.GetBytes(response.StatusCode.ToString());
            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/plain";

            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }

        private static string GetStatusCodeNameFromRequest(HttpListenerContext context)
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

        private static void SetInformationResponse(HttpListenerResponse response)
        {
            response.StatusCode = 101;
        }

        private static void SetSuccessResponse(HttpListenerResponse response)
        {
            response.StatusCode = 200;
        }

        private static void SetRedirectionResponse(HttpListenerResponse response)
        {
            response.StatusCode = 302;
        }

        private static void SetClientErrorResponse(HttpListenerResponse response)
        {
            response.StatusCode = 404;
        }

        private static void SetServerErrorResponse(HttpListenerResponse response)
        {
            response.StatusCode = 503;
        }
    }
}
