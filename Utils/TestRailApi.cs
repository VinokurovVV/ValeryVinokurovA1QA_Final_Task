using Gurock.TestRail;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Final_Task.Utils
{
    class TestRailApi
    {
        public static APIClient GetAPIClient(string testRailUrl, string login, string password)
        {
            APIClient client = new APIClient(testRailUrl)
            {
                User = login,
                Password = password
            };

            return client;
        }

        public static string PostAddSuiteAndGetSuiteID(APIClient client)
        {
            var data = new Dictionary<string, object>
            {
                { "name", "FinalTaskTestSuite"}
            };

            JObject jsonData = (JObject)client.SendPost("add_suite/" + JsonReader.GetParameter("projectID"), data);

            return (string)jsonData["id"];
        }

        public static string PostAddSectionAndGetSectionID(APIClient client, string suiteID)
        {
            var data = new Dictionary<string, object>
            {
                { "suite_id", suiteID},
                { "name", "FinalTaskTestSection"}
            };

            JObject jsonData = (JObject)client.SendPost("add_section/" + JsonReader.GetParameter("projectID"), data);

            return (string)jsonData["id"];
        }

        public static string PostAddRunAndGetRunID(APIClient client, string suiteID, string caseID)
        {
            string[] caseIdarray = { caseID };
            var data = new Dictionary<string, object>
            {
                { "suite_id", suiteID},
                { "name", "FinalTaskTestRun"},
                { "case_ids", caseIdarray}
            };

            JObject jsonData = (JObject)client.SendPost("add_run/" + JsonReader.GetParameter("projectID"), data);

            return (string)jsonData["id"];
        }

        public static string PostAddResultForCaseAndGetResultID(APIClient client, int status, string comment, string runID, string caseId)
        {
            var data = new Dictionary<string, object>
            {
                { "status_id", status},
                { "comment", comment },
            };

            JObject c = (JObject)client.SendPost("add_result_for_case/" + runID + "/" + caseId, data);

            return (string)c["id"];
        }

        public static void PostAddAttachmentToResult(APIClient client, string resultID)
        {
            client.SendPost("add_attachment_to_result/" + resultID, "../../../Resources//ScreenshotOfCurrentPage.png");
        }

        public static string PostAddCaseonAndGetCaseID(APIClient client, string sectionID)
        {
            var data = new Dictionary<string, object>
            {
                { "title", "FinalTaskTestCase" },
                { "type_id", 1 },
                { "priority_id", 3},
                { "estimate", "1m" }
            };

            JObject jsonData = (JObject)client.SendPost("add_case/" + sectionID, data);

            return (string)jsonData["id"];
        }
    }
}
