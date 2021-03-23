namespace Final_Task.Utils
{
    class UrlBuilder
    {
        public static string GetUrlForBasicAuthorization(string username, string password)
        {
            string url = string.Format("http://{0}:{1}@localhost:8080/web", username, password);

            return url;
        }

        public static string GetUrlForTokenGetPostRequest(string variant = "2")
        {
            string url = string.Format("http://localhost:8080/api/token/get?variant={0}", variant);

            return url;
        }

        public static string GetUrlForTestGetXMLPostRequest(string progectId = "1")
        {
            string url = string.Format("http://localhost:8080/api/test/get/xml?projectId={0}", progectId);

            return url;
        }
    }
}
