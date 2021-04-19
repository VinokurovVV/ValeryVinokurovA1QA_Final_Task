using System.Collections.Generic;
using System.Xml.Serialization;

namespace Final_Task.Models
{
    [XmlRoot(xmlRoot, Namespace = "")]
    public class XmlTestsModel
    {
        public const string xmlRoot = "tests";
        [XmlElement("test")]
        public List<TestModel> TestsList { get; set; }
    }
}
