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
            browser.GoTo(UrlBuilder.GetUrlForBasicAuthorization(JsonReader.GetParameter("username"), JsonReader.GetParameter("password")));
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
