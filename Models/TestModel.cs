using System;
using System.Xml.Serialization;

namespace Final_Task.Models
{
    [XmlType("test")]
    public class TestModel
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("method")]
        public string Method { get; set; }
        [XmlElement("status")]
        public string Status { get; set; }
        [XmlElement("startTime")]
        public string StartTime { get; set; }
        [XmlElement("endTime")]
        public string EndTime { get; set; }
        [XmlElement("duration")]
        public string Duration { get; set; }
        [XmlIgnore]
        public string Enviroment { get; set; }
        [XmlIgnore]
        public string Browser { get; set; }
        [XmlIgnore]
        public string PathToScreenshot { get; set; }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (GetType() != other.GetType())
                return false;
            TestModel anotherTest = (TestModel)other;
            if (!Name.Equals(anotherTest.Name))
                return false;
            else if (!Method.Equals(anotherTest.Method))
                return false;
            else if (!Status.ToUpper().Equals(anotherTest.Status.ToUpper()))
                return false;
            else if (!DateTime.Parse(StartTime).Equals(DateTime.Parse(anotherTest.StartTime)))
                return false;
            else if (!DateTime.Parse(Duration).Equals(DateTime.Parse(anotherTest.Duration)))
                return false;
            else if (EndTime == null && (anotherTest.EndTime == null || anotherTest.EndTime.Trim().Equals(string.Empty)))
                return true;
            else if (!DateTime.Parse(EndTime).Equals(DateTime.Parse(anotherTest.EndTime)))
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + (Name != null ? Name.GetHashCode() : 0);
            hash = (hash * 7) + (Method != null ? Method.GetHashCode() : 0);
            hash = (hash * 7) + (Status != null ? Status.GetHashCode() : 0);
            hash = (hash * 7) + (StartTime != null ? StartTime.GetHashCode() : 0);
            hash = (hash * 7) + (Duration != null ? Duration.GetHashCode() : 0);
            hash = (hash * 7) + (EndTime != null ? EndTime.GetHashCode() : 0);

            return hash;
        }
    }
}
