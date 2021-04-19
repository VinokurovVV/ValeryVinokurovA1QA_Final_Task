using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Final_Task.Pages
{
    public class AddProjectPage : Form
    {
        private ITextBox ProjectNameTextBox => ElementFactory.GetTextBox(By.XPath("//input[@id='projectName']"), "projectNameTextBox");
        private IButton SaveProjectButton => ElementFactory.GetButton(By.XPath("//button[@type='submit']"), "saveProjectButton");
        private ILabel ResultLabel => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'alert-success')]"), "resultLabel");

        public AddProjectPage() : base(By.XPath("//form[@id='addProjectForm']"), "AddProjectPage")
        {

        }

        public bool IsPageDisplayed()
        {
            return State.WaitForDisplayed();
        }

        public AddProjectPage SendProgectName(string projectName)
        {
            ProjectNameTextBox.SendKeys(projectName);

            return this;
        }

        public void SaveProjectName()
        {
            SaveProjectButton.Click();
        }

        public string GetTextFromPageResultLabel()
        {
            return ResultLabel.Text;
        }
    }
}
