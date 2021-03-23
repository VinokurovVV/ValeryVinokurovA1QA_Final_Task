using Aquality.Selenium.Browsers;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Final_Task.Pages
{
    public class MainPage : Form
    {
        private readonly IButton nexageButton = ElementFactory.GetButton(By.XPath("//a[@href='allTests?projectId=1']"), "nexageButton");
        private readonly IButton addProgectButton = ElementFactory.GetButton(By.XPath("//a[contains(@class,'btn-primary')]"), "addProgectButton");

        public MainPage() : base(By.XPath("//div[contains(@class,'main-container')]"), "MainPage")
        {

        }

        public bool IsPageDisplayed()
        {
            return new MainPage().State.WaitForDisplayed();
        }

        public bool IsVariantInFooterCorrect(string variant = "2")
        {
            string XPath = string.Format("//span[contains(text(),'Version: {0}')]", variant);
            ILabel footerLabel = ElementFactory.GetLabel(By.XPath(XPath), "footerLabel");

            return footerLabel.State.IsDisplayed;
        }

        public NexageProjectPage ClickOnNexageButton()
        {
            nexageButton.Click();

            return new NexageProjectPage();
        }

        public AddProjectPage ClickOnAddProgectButton()
        {
            addProgectButton.WaitAndClick();
            AqualityServices.Browser.Tabs().SwitchToTab(1);

            return new AddProjectPage();
        }

        public MainPage SelectProgect(string projectName)
        {
            string selectedProgectXPath = string.Format("//a[@class='list-group-item' and contains(text(),'{0}')]", projectName);
            IButton progectButton = ElementFactory.GetButton(By.XPath(selectedProgectXPath), "progectButton");
            progectButton.Click();

            return this;
        }

        public bool CheckThatNewProjectNameIsDisplaeyd(string projectName)
        {
            string newProjectNameXPath = string.Format("//a[@class='list-group-item' and contains(text(),'{0}')]", projectName);
            ILabel newProjectName = ElementFactory.GetLabel(By.XPath(newProjectNameXPath), "newProjectName");

            return newProjectName.State.IsDisplayed;
        }
    }
}
