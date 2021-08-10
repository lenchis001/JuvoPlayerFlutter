using System;
using System.IO;
using System.Threading.Tasks;


namespace JuvoLogger
{
    public class WebLogger : LoggerBase
    {
        public WebLogger(string channel, LogLevel level) : base(channel, level)
        {
        }

        public override void PrintLog(LogLevel level, string message, string file, string method, int line)
        {
            Task.Run(() => LogAsync("Log level " + level + " Message: " + message + " Method:" + method + " Line: " + line)).Wait();
        }

         private static Task LogAsync(string message) {
            var httpClient = new System.Net.Http.HttpClient();

            var content = new System.Net.Http.StringContent("{ \"message\": \"" + message + "\" }");
            content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");

            return httpClient.SendAsync(new System.Net.Http.HttpRequestMessage{
                Method = System.Net.Http.HttpMethod.Post,
                Content = content,
                RequestUri = new Uri("http://192.168.43.230:4004/log")
            });
        }
    }
}
