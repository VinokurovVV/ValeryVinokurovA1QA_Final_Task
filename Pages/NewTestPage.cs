using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Final_Task.Utils;
using OpenQA.Selenium;

namespace Final_Task.Pages
{
    public class NewTestPage : Form
    {
        private readonly ILink screenshotLink = ElementFactory.GetLink(By.XPath("//img[@class='thumbnail']"), "screenshotLink");

        public NewTestPage() : base(By.XPath("//table[@class='table']"), "NewProjectPage")
        {

        }

        public string getScreenshotBase64String()
        {
            string screenshotBase64StringFromWeb = screenshotLink.GetAttribute("src");
            string result = StringUtils.CorrectBase64StringFromWebForComparison(screenshotBase64StringFromWeb);

            return result;
        }

        public bool IsTheFieldFilledCorrect(string text)
        {
            string commonInfoXPath = string.Format("//p[@class='list-group-item-text' and contains(text(),'{0}')]", text);
            var commonInfoLabels = ElementFactory.FindElements<ILabel>(By.XPath(commonInfoXPath), "commonInfoLabel");
            bool isSaved = false;

            foreach (var label in commonInfoLabels)
            {
                if (label.Text.Contains(text))
                {
                    isSaved = true;
                }
            }

            return isSaved;
        }

        public bool IsTheStatusFieldFilledCorrect(string text)
        {
            string statusInfoXPath = string.Format("//span[contains(@class,'label') and contains(text(),'{0}')]", text);
            ILabel statusInfoLabel = ElementFactory.GetLabel(By.XPath(statusInfoXPath), "statusInfoLabel");

            return statusInfoLabel.Text.Contains(text);
        }
    }
}
