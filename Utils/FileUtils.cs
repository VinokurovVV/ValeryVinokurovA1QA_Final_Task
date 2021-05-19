using System;
using System.IO;

namespace Final_Task.Utils
{
    class FileUtils
    {
        public static readonly string pathToTestDataFile = @"/Resources/TestData.json";
        public static readonly string pathToTestScreenshot = @"/Resources/ScreenshotOfCurrentPage.png";

        public static string GetTestFilePath(string filePath)
        {
            string dirName = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo parentDir = new FileInfo(dirName).Directory.Parent.Parent.Parent;
            return parentDir.FullName + filePath;
        }

        public static string GetBase64String(string path)
        {
            byte[] fileContent = File.ReadAllBytes(path);

            return Convert.ToBase64String(fileContent, 0, fileContent.Length);
        }
    }
}
