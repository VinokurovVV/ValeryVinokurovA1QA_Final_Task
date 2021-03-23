using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Final_Task.Utils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Final_Task.Pages
{
    public class NexageProjectPage : Form
    {
        private readonly IList<IButton> listOfTestsOnPage = ElementFactory.FindElements<IButton>(By.XPath("//table[@class='table']//tr"), "listOfTestsOnPage");

        public NexageProjectPage() : base(By.XPath("//canvas[@class='flot-overlay']"), "NexageProjectPage")
        {

        }

        public bool IsPageDisplayed()
        {
            return new NexageProjectPage().State.WaitForDisplayed();
        }

        public int GetAmountOfTestsOnPage()
        {
            return listOfTestsOnPage.Count;
        }

        public string GetTestAttribute(int numberOfTr, int numberOfTd)
        {
            string testAttributeXPath = string.Format("//table[@class='table']/tbody/tr[{0}]/td[{1}]", numberOfTr, numberOfTd);
            ILabel testAttributeLabel = ElementFactory.GetLabel(By.XPath(testAttributeXPath), "testAttributeLabel");

            return testAttributeLabel.Text;
        }

        public bool IsTestsOnPageAreInApiResponse(string apiResponse)
        {
            bool isTestOnPage = false;
            for (int i = 2; i < Convert.ToInt32(JsonReader.GetParameter("nmbOfTestsOnPage")); i++)
            {
                if (apiResponse.Contains(GetTestAttribute(i, Convert.ToInt32(JsonReader.GetParameter("tdNumberForTestName")))))
                {
                    isTestOnPage = true;
                }
                else
                {
                    isTestOnPage = false;
                }
            }

            return isTestOnPage;
        }

        public bool IsTestsSortedByDescendingDate()
        {
            List<DateTime> dateList = new List<DateTime>();
            for (int i = 2; i <= Convert.ToInt32(JsonReader.GetParameter("nmbOfTestsOnPage")); i++)
            {
                dateList.Add(DateTime.Parse(GetTestAttribute(i, Convert.ToInt32(JsonReader.GetParameter("tdNumberForStartTime")))));
            }

            bool isSorted = true;
            for (int i = 0; i < dateList.Count - 1; i++)
            {
                if (dateList[i].CompareTo(dateList[i + 1]) < 0)
                {
                    isSorted = false;
                    break;
                }
            }

            return isSorted;
        }
    }
}
