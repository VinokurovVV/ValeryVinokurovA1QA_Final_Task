using System;
using System.Text;

namespace Final_Task.Utils
{
    class StringUtils
    {
        public const string BASE64_WEB_STRING_START = "data:image/png;base64,";
        public const string EMPTY_STRING = "";

        public static string GenerateRandomString(int size = 7)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();

            char character;

            for (int i = 0; i < size; i++)
            {
                character = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                stringBuilder.Append(character);
            }

            return stringBuilder.ToString().ToLower();
        }

        public static string CorrectBase64StringFromWebForComparison(string screenshotBase64StringFromWeb)
        {
            return screenshotBase64StringFromWeb.Replace(BASE64_WEB_STRING_START, EMPTY_STRING);
        }
    }
}
