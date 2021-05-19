using Final_Task.Models;
using System.IO;
using System.Net;

namespace Final_Task.Utils
{
    class ApiUtils
    {
        public static PostRequestModel GetPostRequestModel(string url, string method = "POST", string contentType = "application/xml")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Method = method;
            request.ContentType = contentType;
            request.Accept = contentType;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);

            return new PostRequestModel() { StatusCode = response.StatusCode, ContentType = response.ContentType, Response = reader.ReadToEnd() };
        }
    }
}
