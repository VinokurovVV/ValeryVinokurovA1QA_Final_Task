using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Final_Task.Utils;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Final_Task.Pages
{
    public class NewTestPage : Form
    {
        private ILink ScreenshotLink => ElementFactory.GetLink(By.XPath("//img[@class='thumbnail']"), "screenshotLink");
        private IList<ILabel> CommonInfoLabels(string text) => ElementFactory.FindElements<ILabel>(By.XPath($"//p[@class='list-group-item-text' and contains(text(),'{text}')]"), "commonInfoLabel");
        private ILabel StatusInfoLabel(string text) => ElementFactory.GetLabel(By.XPath($"//span[contains(@class,'label') and contains(text(),'{text}')]"), "statusInfoLabel");


        public NewTestPage() : base(By.XPath("//table[@class='table']"), "NewProjectPage")
        {

        }

        public string getScreenshotBase64String()
        {
            string screenshotBase64StringFromWeb = ScreenshotLink.GetAttribute("src");
            string result = StringUtils.CorrectBase64StringFromWebForComparison(screenshotBase64StringFromWeb);

            return result;
        }

        public bool IsTheFieldFilledCorrect(string text)
        {
            bool isSaved = false;

            foreach (var label in CommonInfoLabels(text))
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
            return StatusInfoLabel(text).Text.Contains(text);
        }
    }
}
