using System;
using System.IO;

namespace Final_Task.Utils
{
    class FileUtil
    {
        public static string GetBase64String(string path)
        {
            byte[] fileContent = File.ReadAllBytes(path);

            return Convert.ToBase64String(fileContent, 0, fileContent.Length);
        }
    }
}
