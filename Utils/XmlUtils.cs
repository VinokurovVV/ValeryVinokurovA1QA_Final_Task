using System;
using System.IO;
using System.Xml.Serialization;

namespace Final_Task.Utils
{
    class XmlUtils
    {
        public static T ConvertXmlStringToObject<T>(string xmlString)
        {
            if (xmlString == null) return default;
            if (xmlString == string.Empty) return (T)Activator.CreateInstance(typeof(T));

            StringReader reader = new StringReader(xmlString);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(reader);
        }

        public static string SetXmlRootToXmlString(string rootName, string xmlString)
        {
            return string.Format($"<{rootName}>" + xmlString + $"</{rootName}>");
        }
    }
}
