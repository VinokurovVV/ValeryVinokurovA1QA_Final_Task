using Gurock.TestRail;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Final_Task.Utils
{
    class TestRailApi
    {
        public enum TestRailParameter
        {
            Suite_id,
            Name,
            Case_ids,
            Status_id,
            Comment,
            Title,
            Type_id,
            Priority_id,
            Estimate
        };

        public static string TestRailParameterString(TestRailParameter paramString) => paramString switch
        {
            TestRailParameter.Suite_id => "suite_id",
            TestRailParameter.Name => "name",
            TestRailParameter.Case_ids => "case_ids",
            TestRailParameter.Status_id => "status_id",
            TestRailParameter.Comment => "comment",
            TestRailParameter.Title => "title",
            TestRailParameter.Type_id => "type_id",
            TestRailParameter.Priority_id => "priority_id",
            TestRailParameter.Estimate => "estimate",
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(paramString)),
        };

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
                { TestRailParameterString(TestRailParameter.Name), "FinalTaskTestSuite"}
            };

            JObject jsonData = (JObject)client.SendPost("add_suite/" + JsonReader.GetParameter("projectID"), data);

            return (string)jsonData["id"];
        }

        public static string PostAddSectionAndGetSectionID(APIClient client, string suiteID)
        {
            var data = new Dictionary<string, object>
            {
                { TestRailParameterString(TestRailParameter.Suite_id), suiteID},
                { TestRailParameterString(TestRailParameter.Name), "FinalTaskTestSection"}
            };

            JObject jsonData = (JObject)client.SendPost("add_section/" + JsonReader.GetParameter("projectID"), data);

            return (string)jsonData["id"];
        }

        public static string PostAddRunAndGetRunID(APIClient client, string suiteID, string caseID)
        {
            string[] caseIdarray = { caseID };
            var data = new Dictionary<string, object>
            {
                { TestRailParameterString(TestRailParameter.Suite_id), suiteID},
                { TestRailParameterString(TestRailParameter.Name), "FinalTaskTestRun"},
                { TestRailParameterString(TestRailParameter.Case_ids), caseIdarray}
            };

            JObject jsonData = (JObject)client.SendPost("add_run/" + JsonReader.GetParameter("projectID"), data);

            return (string)jsonData["id"];
        }

        public static string PostAddResultForCaseAndGetResultID(APIClient client, int status, string comment, string runID, string caseId)
        {
            var data = new Dictionary<string, object>
            {
                { TestRailParameterString(TestRailParameter.Suite_id), status},
                { TestRailParameterString(TestRailParameter.Comment), comment },
            };

            JObject c = (JObject)client.SendPost("add_result_for_case/" + runID + "/" + caseId, data);

            return (string)c["id"];
        }

        public static void PostAddAttachmentToResult(APIClient client, string resultID, string filePath)
        {
            client.SendPost("add_attachment_to_result/" + resultID, filePath);
        }

        public static string PostAddCaseonAndGetCaseID(APIClient client, string sectionID)
        {
            var data = new Dictionary<string, object>
            {
                { TestRailParameterString(TestRailParameter.Title), "FinalTaskTestCase" },
                { TestRailParameterString(TestRailParameter.Type_id), 1 },
                { TestRailParameterString(TestRailParameter.Priority_id), 3},
                { TestRailParameterString(TestRailParameter.Estimate), "1m" }
            };

            JObject jsonData = (JObject)client.SendPost("add_case/" + sectionID, data);

            return (string)jsonData["id"];
        }
    }
}
