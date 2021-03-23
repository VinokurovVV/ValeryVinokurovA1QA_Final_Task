using Aquality.Selenium.Browsers;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Final_Task.Pages
{
    public class NewTestConfigForm : Form
    {
        private readonly ITextBox testNameTextBox = ElementFactory.GetTextBox(By.XPath("//input[@id='testName']"), "testNameTextBox");
        private readonly ITextBox testStatusTextBox = ElementFactory.GetTextBox(By.XPath("//select[@id='testStatus']"), "testStatusTextBox");
        private readonly ITextBox testMethodTextBox = ElementFactory.GetTextBox(By.XPath("//input[@id='testMethod']"), "testMethodTextBox");
        private readonly ITextBox startTimeTextBox = ElementFactory.GetTextBox(By.XPath("//input[@id='startTime']"), "startTimeTextBox");
        private readonly ITextBox endTimeTextBox = ElementFactory.GetTextBox(By.XPath("//input[@id='endTime']"), "endTimeTextBox");
        private readonly ITextBox enviromentTextBox = ElementFactory.GetTextBox(By.XPath("//input[@id='environment']"), "enviromentTextBox");
        private readonly ITextBox browserTextBox = ElementFactory.GetTextBox(By.XPath("//input[@id='browser']"), "browserTextBox");
        private readonly IButton attachmentButton = ElementFactory.GetButton(By.XPath("//input[@id='attachment']"), "attachmentButton");
        private readonly IButton saveTestButton = ElementFactory.GetButton(By.XPath("//button[contains(@class,'btn-primary') and @type='button']"), "saveTestButton");
        private readonly ILabel resultLabel = ElementFactory.GetLabel(By.XPath("//div[contains(@class,'alert-success')]"), "resultLabel");
        private readonly ILabel pageBodyLabel = ElementFactory.GetLabel(By.XPath("//body"), "pageBodyLabel");

        public NewTestConfigForm() : base(By.XPath("//div[@class='modal-content']"), "NewTestForm")
        {

        }

        public NewTestConfigForm SetParametersToTest(string testName, string testStatus, string testMethod, string startTime, string endTime, string enviroment, string browser)
        {
            testNameTextBox.SendKeys(testName);
            testStatusTextBox.SendKeys(testStatus);
            testMethodTextBox.SendKeys(testMethod);
            startTimeTextBox.SendKeys(startTime);
            endTimeTextBox.SendKeys(endTime);
            enviromentTextBox.SendKeys(enviroment);
            browserTextBox.SendKeys(browser);
            attachmentButton.SendKeys("C:/Файлы по a1qa/FinalTask/Final_Task/Resources/ScreenshotOfCurrentPage.png");

            return this;
        }

        public NewTestConfigForm SaveFileScreenshotOfCurrentPage()
        {
            pageBodyLabel.State.WaitForExist();
            Screenshot screenshot = pageBodyLabel.GetElement().GetScreenshot();
            screenshot.SaveAsFile("../../../Resources//ScreenshotOfCurrentPage.png");

            return this;
        }

        public string GetTextFromPageResultLabel()
        {
            return resultLabel.Text;
        }

        public NewTestConfigForm ClickOnSaveTestButton()
        {
            saveTestButton.Click();

            return this;
        }

        public NewTestConfigForm CloseThePopUp()
        {
            Actions actions = new Actions(AqualityServices.Browser.Driver);
            actions.MoveByOffset(100, 100).Click().Build().Perform();

            return this;
        }
    }
}
