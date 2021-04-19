using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Final_Task.Pages
{
    public class NewProjectPage : Form
    {
        public NewTestConfigForm newTestConfigForm;

        private IButton AddTestButton => ElementFactory.GetButton(By.XPath("//button[contains(@class,'btn-primary')]"), "addTestButton");
        private ILabel TestInfoLabel(string testName) => ElementFactory.GetLabel(By.XPath($"//a[contains(@href,'testInfo') and contains(text(),'{testName}')]"), "testInfoLabel");
        private IButton NewTestButton(string testName) => ElementFactory.GetButton(By.XPath($"//a[contains(@href,'testInfo') and contains(text(),'{testName}')]"), "newTestButton");

        public NewProjectPage() : base(By.XPath("//canvas[@class='flot-overlay']"), "NewProjectPage")
        {
            newTestConfigForm = new NewTestConfigForm();
        }

        public NewProjectPage ClickOnAddTestButton()
        {
            AddTestButton.Click();

            return this;
        }

        public bool IsNewTestDisplayed(string testName)
        {
            return TestInfoLabel(testName).State.WaitForDisplayed();
        }

        public void ClickOnNewTestButton(string testName)
        {
            NewTestButton(testName).Click();
        }
    }
}
