using Aquality.Selenium.Browsers;
using Final_Task.Utils;
using NUnit.Framework;

namespace Final_Task.Tests
{
    class BaseTest
    {
        private readonly Browser browser = AqualityServices.Browser;

        [SetUp]
        public void Setup()
        {
            browser.Maximize();
            browser.GoTo(UrlBuilder.GetUrlForBasicAuthorization(
                JsonReader.GetParameter("username") ?? TestContext.Parameters["username"]?.ToString(),
                JsonReader.GetParameter("password") ?? TestContext.Parameters["password"]?.ToString(),
                JsonReader.GetParameter("url")));
            browser.WaitForPageToLoad();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (browser != null)
            {
                browser.Quit();
            }
        }
    }
}
