using Aquality.Selenium.Browsers;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Final_Task.Pages
{
    public class AddProjectPage : Form
    {
        private readonly ITextBox projectNameTextBox = ElementFactory.GetTextBox(By.XPath("//input[@id='projectName']"), "projectNameTextBox");
        private readonly IButton saveProjectButton = ElementFactory.GetButton(By.XPath("//button[@type='submit']"), "saveProjectButton");
        private readonly ILabel resultLabel = ElementFactory.GetLabel(By.XPath("//div[contains(@class,'alert-success')]"), "resultLabel");

        public AddProjectPage() : base(By.XPath("//form[@id='addProjectForm']"), "AddProjectPage")
        {

        }

        public bool IsPageDisplayed()
        {
            return new AddProjectPage().State.WaitForDisplayed();
        }

        public AddProjectPage SendProgectName(string projectName)
        {
            projectNameTextBox.SendKeys(projectName);

            return this;
        }

        public AddProjectPage SaveProjectName()
        {
            saveProjectButton.Click();

            return this;
        }

        public string GetTextFromPageResultLabel()
        {
            return resultLabel.Text;
        }

        public MainPage CloseAddProjectPage()
        {
            AqualityServices.Browser.Tabs().CloseTab();
            AqualityServices.Browser.Tabs().SwitchToLastTab();

            return new MainPage();
        }
    }
}
