namespace Final_Task.Utils
{
    class UrlBuilder
    {
        public static string GetUrlForBasicAuthorization(string username, string password, string url)
        {
            string urlForBasicAuthorization = string.Format($"http://{username}:{password}@{url}/web");

            return urlForBasicAuthorization;
        }

        public static string GetUrlForTokenGetPostRequest(string url, string variant)
        {
            string urlForTokenGetPostRequest = string.Format($"http://{url}/api/token/get?variant={variant}");

            return urlForTokenGetPostRequest;
        }

        public static string GetUrlForTestGetXMLPostRequest(string url, string progectId)
        {
            string urlForTestGetXMLPostRequest = string.Format($"http://{url}/api/test/get/xml?projectId={progectId}");

            return urlForTestGetXMLPostRequest;
        }
    }
}
