using Aquality.Selenium.Browsers;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Final_Task.Models;
using Final_Task.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Final_Task.Pages
{
    public class NewTestConfigForm : Form
    {
        private ITextBox TestNameTextBox => ElementFactory.GetTextBox(By.XPath("//input[@id='testName']"), "testNameTextBox");
        private ITextBox TestStatusTextBox => ElementFactory.GetTextBox(By.XPath("//select[@id='testStatus']"), "testStatusTextBox");
        private ITextBox TestMethodTextBox => ElementFactory.GetTextBox(By.XPath("//input[@id='testMethod']"), "testMethodTextBox");
        private ITextBox StartTimeTextBox => ElementFactory.GetTextBox(By.XPath("//input[@id='startTime']"), "startTimeTextBox");
        private ITextBox EndTimeTextBox => ElementFactory.GetTextBox(By.XPath("//input[@id='endTime']"), "endTimeTextBox");
        private ITextBox EnviromentTextBox => ElementFactory.GetTextBox(By.XPath("//input[@id='environment']"), "enviromentTextBox");
        private ITextBox BrowserTextBox => ElementFactory.GetTextBox(By.XPath("//input[@id='browser']"), "browserTextBox");
        private IButton AttachmentButton => ElementFactory.GetButton(By.XPath("//input[@id='attachment']"), "attachmentButton");
        private IButton SaveTestButton => ElementFactory.GetButton(By.XPath("//button[contains(@class,'btn-primary') and @type='button']"), "saveTestButton");
        private ILabel ResultLabel => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'alert-success')]"), "resultLabel");
        private ILabel PageBodyLabel => ElementFactory.GetLabel(By.XPath("//body"), "pageBodyLabel");

        public NewTestConfigForm() : base(By.XPath("//div[@class='modal-content']"), "NewTestForm")
        {

        }

        public NewTestConfigForm SetParametersToTest(TestModel newTest)
        {
            TestNameTextBox.SendKeys(newTest.Name);
            TestStatusTextBox.SendKeys(newTest.Status);
            TestMethodTextBox.SendKeys(newTest.Method);
            StartTimeTextBox.SendKeys(newTest.StartTime);
            EndTimeTextBox.SendKeys(newTest.EndTime);
            EnviromentTextBox.SendKeys(newTest.Enviroment);
            BrowserTextBox.SendKeys(newTest.Browser);
            AttachmentButton.SendKeys(newTest.PathToScreenshot);

            return this;
        }

        public NewTestConfigForm SaveFileScreenshotOfCurrentPage()
        {
            PageBodyLabel.State.WaitForExist();
            Screenshot screenshot = PageBodyLabel.GetElement().GetScreenshot();
            screenshot.SaveAsFile(FileUtils.GetTestFilePath(FileUtils.pathToTestScreenshot));

            return this;
        }

        public string GetTextFromPageResultLabel()
        {
            return ResultLabel.Text;
        }

        public void ClickOnSaveTestButton()
        {
            SaveTestButton.Click();
        }

        public void CloseThePopUp()
        {
            Actions actions = new Actions(AqualityServices.Browser.Driver);
            actions.MoveByOffset(100, 100).Click().Build().Perform();
        }
    }
}
