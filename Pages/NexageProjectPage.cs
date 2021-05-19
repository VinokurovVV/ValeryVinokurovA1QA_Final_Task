using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Final_Task.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Final_Task.Pages
{
    public class NexageProjectPage : Form
    {
        private ILabel TestNameAttribute(string numberOfRow) => ElementFactory.GetLabel(By.XPath($"//table[@class='table']/tbody/tr[{numberOfRow}]/td[1]"), "testNameAttribute");
        private ILabel TestMethodAttribute(string numberOfRow) => ElementFactory.GetLabel(By.XPath($"//table[@class='table']/tbody/tr[{numberOfRow}]/td[2]"), "testNameAttribute");
        private ILabel TestStatusAttribute(string numberOfRow) => ElementFactory.GetLabel(By.XPath($"//table[@class='table']/tbody/tr[{numberOfRow}]/td[3]"), "testNameAttribute");
        private ILabel TestStartTimeAttribute(string numberOfRow) => ElementFactory.GetLabel(By.XPath($"//table[@class='table']/tbody/tr[{numberOfRow}]/td[4]"), "testNameAttribute");
        private ILabel TestEndTimeAttribute(string numberOfRow) => ElementFactory.GetLabel(By.XPath($"//table[@class='table']/tbody/tr[{numberOfRow}]/td[5]"), "testNameAttribute");
        private ILabel TestDurationAttribute(string numberOfRow) => ElementFactory.GetLabel(By.XPath($"//table[@class='table']/tbody/tr[{numberOfRow}]/td[6]"), "testNameAttribute");

        public NexageProjectPage() : base(By.XPath("//li[contains(text(),'Nexage')]"), "NexageProjectPage")
        {

        }

        public bool IsPageDisplayed()
        {
            return State.WaitForDisplayed();
        }

        public int GetAmountOfTestsOnPage()
        {
            return ElementFactory.FindElements<IButton>(By.XPath("//table[@class='table']//tr"), "listOfTestsOnPage").Count;
        }

        public TestModel GetTestByNumberOfRow(string numberOfRow)
        {
            return new TestModel()
            {
                Name = TestNameAttribute(numberOfRow).Text,
                Method = TestMethodAttribute(numberOfRow).Text,
                Status = TestStatusAttribute(numberOfRow).Text.ToUpper(),
                StartTime = TestStartTimeAttribute(numberOfRow).Text,
                EndTime = TestEndTimeAttribute(numberOfRow).Text,
                Duration = TestDurationAttribute(numberOfRow).Text
            };
        }

        public List<TestModel> GetTestsList()
        {
            List<TestModel> testsList = new List<TestModel>();

            for (int i = 2; i < GetAmountOfTestsOnPage(); i++)
            {
                testsList.Add(GetTestByNumberOfRow(i.ToString()));
            }

            return testsList;
        }

        public List<DateTime> GetTestsDateList()
        {
            List<DateTime> testsDateList = new List<DateTime>();

            for (int i = 2; i < GetAmountOfTestsOnPage(); i++)
            {
                testsDateList.Add(DateTime.Parse(TestStartTimeAttribute(i.ToString()).Text));
            }

            return testsDateList;
        }
    }
}
