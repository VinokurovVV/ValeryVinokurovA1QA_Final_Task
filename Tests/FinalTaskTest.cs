using Aquality.Selenium.Browsers;
using Final_Task.Pages;
using Final_Task.Utils;
using Gurock.TestRail;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

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
            string token = ApiUtils.PostRequest(UrlBuilder.GetUrlForTokenGetPostRequest());
            Assert.AreNotEqual(token, null, "Token are not generated.");

            AqualityServices.Logger.Info(
                "Using a cookie, pass the token generated in step 1." +
                " Refresh the page.");
            CookieUtils.AddCookie("token", token);
            AqualityServices.Browser.Refresh();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(mainPage.IsPageDisplayed(), "Page with projects is not displayed.");
                Assert.IsTrue(mainPage.IsVariantInFooterCorrect(), "Invalid variant number in footer.");
            });

            AqualityServices.Logger.Info(
                "Go to the Nexage project page." +
                " Send request to the api to get a list of tests in XML format.");
            mainPage.ClickOnNexageButton();
            string apiResponseWithTestsInXML = ApiUtils.PostRequest(UrlBuilder.GetUrlForTestGetXMLPostRequest());
            Assert.Multiple(() =>
            {
                Assert.IsTrue(nexageProjectPage.IsPageDisplayed(), "NexageProgectPage is not displayed.");
                Assert.IsTrue(nexageProjectPage.IsTestsOnPageAreInApiResponse(apiResponseWithTestsInXML), "Tests on page are not in api response");
                Assert.IsTrue(nexageProjectPage.IsTestsSortedByDescendingDate(), "Tests are not sorted by descending date");
            });
            
            AqualityServices.Logger.Info(
                "Return to the previous page of projects." +
                " Click on + Add.");
            AqualityServices.Browser.GoBack();
            mainPage.ClickOnAddProgectButton();
            Assert.IsTrue(addProjectPage.IsPageDisplayed(), "AddProgectPage is not displayed.");

            AqualityServices.Logger.Info(
                 "In the tab that opens, enter the name of the project and save." +
                 " Close the tab and return to the previous one. Refresh the page");
            string projectName = StringUtils.GenerateRandomString();
            addProjectPage.SendProgectName(projectName).SaveProjectName();
            Assert.AreEqual("Project " + projectName + " saved", addProjectPage.GetTextFromPageResultLabel(), "Project is not saved.");
            addProjectPage.CloseAddProjectPage();
            AqualityServices.Browser.Refresh();
            Assert.IsTrue(mainPage.CheckThatNewProjectNameIsDisplaeyd(projectName), "New project name is not displayed.");

            AqualityServices.Logger.Info(
                "Go to the page of the created project." +
                " Click on + Add." +
                " Fill in the required fields and attach a screenshot of the current page." +
                " Save the test." +
                " To close the pop-up, click outside its window.");
            mainPage.SelectProgect("ifiunpj");
            //mainPage.SelectProgect(projectName);
            newProjectPage.ClickOnAddTestButton().
                           newTestConfigForm.SetParametersToTest(
                JsonReader.GetParameter("testName"),
                JsonReader.GetParameter("testStatus"),
                JsonReader.GetParameter("testMethod"),
                JsonReader.GetParameter("startTime"),
                JsonReader.GetParameter("endTime"),
                JsonReader.GetParameter("enviroment"),
                JsonReader.GetParameter("browser")).
                           SaveFileScreenshotOfCurrentPage().
                           ClickOnSaveTestButton();
            Assert.AreEqual("Test " + JsonReader.GetParameter("testName") + " saved", newProjectPage.newTestConfigForm.GetTextFromPageResultLabel(), "Test is not saved.");
            newProjectPage.newTestConfigForm.CloseThePopUp();
            Assert.IsTrue(newProjectPage.IsNewTestInfoDisplayed(JsonReader.GetParameter("testName")), "New test is not displayed.");

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
                Assert.AreEqual(FileUtil.GetBase64String("../../../Resources//ScreenshotOfCurrentPage.png"), newTestPage.getScreenshotBase64String(), "Screenshot image is not correct");
            });
        }

        [TearDown]
        public void TestTearDown()
        {
            APIClient client = TestRailApi.GetAPIClient(JsonReader.GetParameter("testRailUrl"),
                JsonReader.GetParameter("testRailLogin"),
                JsonReader.GetParameter("testRailPassword"));

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
            TestRailApi.PostAddAttachmentToResult(client, resultID);
        }
    }
}
