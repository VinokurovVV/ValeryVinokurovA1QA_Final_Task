using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Final_Task.Pages
{
    public class MainPage : Form
    {
        private IButton AddProgectButton => ElementFactory.GetButton(By.XPath("//a[contains(@class,'btn-primary')]"), "addProgectButton");
        private ILabel FooterLabel(string variant) => ElementFactory.GetLabel(By.XPath($"//span[contains(text(),'Version: {variant}')]"), "footerLabel");
        private IButton ProgectButton(string progectName) => ElementFactory.GetButton(By.XPath($"//a[@class='list-group-item' and contains(text(),'{progectName}')]"), "progectButton");
        private ILabel NewProjectName(string newProjectName) => ElementFactory.GetLabel(By.XPath($"//a[@class='list-group-item' and contains(text(),'{newProjectName}')]"), "newProjectName");

        public MainPage() : base(By.XPath("//div[contains(@class,'main-container')]"), "MainPage")
        {

        }

        public bool IsPageDisplayed()
        {
            return State.WaitForDisplayed();
        }

        public bool IsVariantInFooterCorrect(string variant)
        {
            return FooterLabel(variant).State.IsDisplayed;
        }

        public void ClickOnAddProgectButton()
        {
            AddProgectButton.WaitAndClick();
        }

        public void SelectProgect(string projectName)
        {
            ProgectButton(projectName).Click();
        }

        public bool CheckThatNewProjectNameIsDisplaeyd(string projectName)
        {
            return NewProjectName(projectName).State.IsDisplayed;
        }

        public string GetProjectIdByProjectName(string projectName)
        {
            string projectHref = ProgectButton(projectName).GetAttribute("href");

            return projectHref.Substring(projectHref.Length - 1);
        }
    }
}
