using Aquality.Selenium.Browsers;
using Final_Task.Models;
using Final_Task.Pages;
using Final_Task.Utils;
using Gurock.TestRail;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Net;

namespace Final_Task.Tests
{
    [TestFixture]
    class FinalTaskTest : BaseTest
    {
        [Test]
        public void TestUnionReportingWeb()
        {
            MainPage mainPage = new MainPage();
            NexageProjectPage nexageProjectPage = new NexageProjectPage();
            AddProjectPage addProjectPage = new AddProjectPage();
            NewProjectPage newProjectPage = new NewProjectPage();
            NewTestPage newTestPage = new NewTestPage();

            AqualityServices.Logger.Info("Send request to the api to get a token according to the option number");
            PostRequestModel apiRequestForToken = ApiUtils.GetPostRequestModel(UrlBuilder.GetUrlForTokenGetPostRequest(JsonReader.GetParameter("variant")));
            Assert.AreEqual(apiRequestForToken.StatusCode, HttpStatusCode.OK, "Post request status code is not correct.");
            string token = apiRequestForToken.Response;
            Assert.AreNotEqual(token, null, "Token are not generated.");

            AqualityServices.Logger.Info(
                "Using a cookie, pass the token generated in step 1." +
                " Refresh the page.");
            CookieUtils.AddCookie("token", token);
            AqualityServices.Browser.Refresh();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(mainPage.IsPageDisplayed(), "Page with projects is not displayed.");
                Assert.IsTrue(mainPage.IsVariantInFooterCorrect(JsonReader.GetParameter("variant")), "Invalid variant number in footer.");
            });

            AqualityServices.Logger.Info(
                "Go to the Nexage project page." +
                " Send request to the api to get a list of tests in XML format.");
            string nexageProjectId = mainPage.GetProjectIdByProjectName("Nexage");
            mainPage.SelectProgect("Nexage");
            PostRequestModel apiRequestForTestsInXML = ApiUtils.GetPostRequestModel(UrlBuilder.GetUrlForTestGetXMLPostRequest(nexageProjectId));
            Assert.Multiple(() =>
            {
                Assert.AreEqual(apiRequestForTestsInXML.StatusCode, HttpStatusCode.OK, "Post request status code is not correct.");
                Assert.AreEqual(apiRequestForTestsInXML.ContentType, JsonReader.GetParameter("testsInXMLResponseContentType"), "Post request content type is not correct.");
            });

            string testsInXml = apiRequestForTestsInXML.Response;
            var testsListFromXml = XmlUtils.ConvertXmlStringToObject<XmlTestsModel>(XmlUtils.SetXmlRootToXmlString(XmlTestsModel.xmlRoot, testsInXml)).TestsList;
            Assert.Multiple(() =>
            {
                Assert.IsTrue(nexageProjectPage.IsPageDisplayed(), "NexageProgectPage is not displayed.");
                var testsDateListFromNexagePage = nexageProjectPage.GetTestsDateList();
                var testsListFromNexagePage = nexageProjectPage.GetTestsList();
                Assert.IsTrue(SortUtils.IsTestsSortedByDescendingDate(testsDateListFromNexagePage), "Tests are not sorted by descending date.");
                Assert.IsTrue(ComparisonUtils.IsListContainsAllItemsFromAnother(testsListFromNexagePage, testsListFromXml), "List from xml does not contain all tests from nexage page.");
            });

            AqualityServices.Logger.Info(
                "Return to the previous page of projects." +
                " Click on + Add.");
            AqualityServices.Browser.GoBack();
            mainPage.ClickOnAddProgectButton();
            AqualityServices.Browser.Tabs().SwitchToTab(1);
            Assert.IsTrue(addProjectPage.IsPageDisplayed(), "AddProgectPage is not displayed.");

            AqualityServices.Logger.Info(
                 "In the tab that opens, enter the name of the project and save." +
                 " Close the tab and return to the previous one. Refresh the page");
            string projectName = StringUtils.GenerateRandomString();
            addProjectPage.SendProgectName(projectName).SaveProjectName();
            Assert.AreEqual("Project " + projectName + " saved", addProjectPage.GetTextFromPageResultLabel(), "Project is not saved.");
            AqualityServices.Browser.Tabs().CloseTab();
            AqualityServices.Browser.Tabs().SwitchToLastTab();
            AqualityServices.Browser.Refresh();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, AqualityServices.Browser.Tabs().TabHandles.Count, "Tab is not closed.");
                Assert.IsTrue(mainPage.CheckThatNewProjectNameIsDisplaeyd(projectName), "New project name is not displayed.");
            });

            AqualityServices.Logger.Info(
                "Go to the page of the created project." +
                " Click on + Add." +
                " Fill in the required fields and attach a screenshot of the current page." +
                " Save the test." +
                " To close the pop-up, click outside its window.");
            mainPage.SelectProgect(projectName);
            TestModel newTest = new TestModel()
            {
                Name = JsonReader.GetParameter("testName"),
                Method = JsonReader.GetParameter("testMethod"),
                Status = JsonReader.GetParameter("testStatus"),
                StartTime = JsonReader.GetParameter("startTime"),
                EndTime = JsonReader.GetParameter("endTime"),
                Enviroment = JsonReader.GetParameter("enviroment"),
                Browser = JsonReader.GetParameter("browser"),
                PathToScreenshot = FileUtils.GetTestFilePath(FileUtils.pathToTestScreenshot)
            };
            newProjectPage.ClickOnAddTestButton().
                           newTestConfigForm.SetParametersToTest(newTest).
                           SaveFileScreenshotOfCurrentPage().ClickOnSaveTestButton();
            Assert.AreEqual("Test " + JsonReader.GetParameter("testName") + " saved", newProjectPage.newTestConfigForm.GetTextFromPageResultLabel(), "Test is not saved.");
            newProjectPage.newTestConfigForm.CloseThePopUp();
            Assert.IsTrue(newProjectPage.IsNewTestDisplayed(JsonReader.GetParameter("testName")), "New test is not displayed.");

            AqualityServices.Logger.Info(
                "Go to the page of the created test." +
                " Check the correctness of the information.");
            newProjectPage.ClickOnNewTestButton(JsonReader.GetParameter("testName"));
            Assert.Multiple(() =>
            {
                Assert.IsTrue(newTestPage.IsTheFieldFilledCorrect(JsonReader.GetParameter("testName")), "Test name is not correct.");
                Assert.IsTrue(newTestPage.IsTheStatusFieldFilledCorrect(JsonReader.GetParameter("testStatus")), "Test status is not correct.");
                Assert.IsTrue(newTestPage.IsTheFieldFilledCorrect(JsonReader.GetParameter("testMethod")), "Test method is not correct.");
                Assert.IsTrue(newTestPage.IsTheFieldFilledCorrect(JsonReader.GetParameter("startTime")), "Start time is not correct.");
                Assert.IsTrue(newTestPage.IsTheFieldFilledCorrect(JsonReader.GetParameter("endTime")), "End time is not correct.");
                Assert.IsTrue(newTestPage.IsTheFieldFilledCorrect(JsonReader.GetParameter("enviroment")), "Enviroment is not correct.");
                Assert.IsTrue(newTestPage.IsTheFieldFilledCorrect(JsonReader.GetParameter("browser")), "Browser is not correct.");
                Assert.AreEqual(FileUtils.GetBase64String(FileUtils.GetTestFilePath(FileUtils.pathToTestScreenshot)), newTestPage.getScreenshotBase64String(), "Screenshot image is not correct");
            });
        }

        [TearDown]
        public void TestTearDown()
        {
            APIClient client = TestRailApi.GetAPIClient(JsonReader.GetParameter("testRailUrl"),
                JsonReader.GetParameter("testRailLogin") ?? TestContext.Parameters["testRailLogin"]?.ToString(),
                JsonReader.GetParameter("testRailPassword") ?? TestContext.Parameters["testRailPassword"]?.ToString());

            int testStatus = (int)Utils.TestStatus.TEST_CASE_UNTESTED_STATUS;
            string comment = TestStatusMessage.TEST_CASE_UNTESTED_MESSAGE;
            if ((TestContext.CurrentContext.Result.Outcome == ResultState.Failure) || (TestContext.CurrentContext.Result.Outcome == ResultState.Error))
            {
                testStatus = (int)Utils.TestStatus.TEST_CASE_FAILED_STATUS;
                comment = TestStatusMessage.TEST_CASE_FAILED_MESSAGE;
            }
            else if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                testStatus = (int)Utils.TestStatus.TEST_CASE_PASSED_STATUS;
                comment = TestStatusMessage.TEST_CASE_PASSED_MESSAGE;
            }

            string suiteID = TestRailApi.PostAddSuiteAndGetSuiteID(client);
            string sectionID = TestRailApi.PostAddSectionAndGetSectionID(client, suiteID);
            string caseID = TestRailApi.PostAddCaseonAndGetCaseID(client, sectionID);
            string runID = TestRailApi.PostAddRunAndGetRunID(client, suiteID, caseID);

            string resultID = TestRailApi.PostAddResultForCaseAndGetResultID(client, testStatus, comment, runID, caseID);
            TestRailApi.PostAddAttachmentToResult(client, resultID, FileUtils.GetTestFilePath(FileUtils.pathToTestScreenshot));
        }
    }
}
