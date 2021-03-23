using System;
using System.IO;
using System.Net;

namespace Final_Task.Utils
{
    class ApiUtils
    {       
        public static string PostRequest(string url, string method = "POST")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Method = method;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            Console.WriteLine(response.StatusCode);
            return reader.ReadToEnd();
        }
    }
}
