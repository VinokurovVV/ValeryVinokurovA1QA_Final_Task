using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Final_Task.Pages
{
    public class NewProjectPage : Form
    {
        public NewTestConfigForm newTestConfigForm;

        private readonly IButton addTestButton = ElementFactory.GetButton(By.XPath("//button[contains(@class,'btn-primary')]"), "addTestButton");

        public NewProjectPage() : base(By.XPath("//canvas[@class='flot-overlay']"), "NewProjectPage")
        {
            newTestConfigForm = new NewTestConfigForm();
        }

        public NewProjectPage ClickOnAddTestButton()
        {
            addTestButton.Click();

            return this;
        }       

        public bool IsNewTestInfoDisplayed(string testName)
        {
            string newTestInfoXPath = string.Format("//a[contains(@href,'testInfo') and contains(text(),'{0}')]", testName);
            ILabel testInfoLabel = ElementFactory.GetLabel(By.XPath(newTestInfoXPath), "testInfoLabel");

            return testInfoLabel.State.WaitForDisplayed();
        }

        public NewTestPage ClickOnNewTestButton(string testName) 
        {
            string newTestXPath = string.Format("//a[contains(@href,'testInfo') and contains(text(),'{0}')]", testName);
            IButton newTestButton = ElementFactory.GetButton(By.XPath(newTestXPath), "newTestButton");
            newTestButton.Click(); 

            return new NewTestPage();
        }
    }
}
