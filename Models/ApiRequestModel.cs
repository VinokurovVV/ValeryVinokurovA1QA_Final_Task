using System.Net;

namespace Final_Task.Models
{
    public class PostRequestModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; }
        public string Response { get; set; }
    }
}
