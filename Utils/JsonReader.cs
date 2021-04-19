using Newtonsoft.Json.Linq;
using System.IO;

namespace Final_Task.Utils
{
    class JsonReader
    {
        public static string GetParameter(string jsonData)
        {
            var data = JObject.Parse(File.ReadAllText(FileUtils.GetTestFilePath(FileUtils.pathToTestDataFile)));
            return (string)data[jsonData];
        }
    }
}
