using Aquality.Selenium.Browsers;
using OpenQA.Selenium;

namespace Final_Task.Utils
{
    class CookieUtils
    {
        public static void AddCookie(string key, string value)
        {
            AqualityServices.Browser.Driver.Manage().Cookies.AddCookie(new Cookie(key, value));
        }
    }
}
