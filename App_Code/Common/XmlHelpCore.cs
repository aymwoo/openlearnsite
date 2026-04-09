using System;
using System.Collections.Generic;
using System.Xml;

namespace LearnSite.Common
{
    public static class XmlHelpCore
    {
        public static string GetValue(string xmlPath, string key, string attributeName = "value")
        {
            string value = "0";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("/LearnSite/website").ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                XmlElement element = (XmlElement)node;
                if (element.GetAttribute("key") == key)
                {
                    value = element.GetAttribute(attributeName);
                    break;
                }
            }

            return value.Trim();
        }

        public static bool SetValue(string xmlPath, string key, string value, string attributeName = "value")
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("/LearnSite/website").ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                XmlElement element = (XmlElement)node;
                if (element.GetAttribute("key") == key)
                {
                    element.SetAttribute(attributeName, value);
                    xmlDoc.Save(xmlPath);
                    return true;
                }
            }

            return false;
        }

        public static string[] GetPipeSeparatedValues(string xmlPath, string key)
        {
            string allFileType = GetValue(xmlPath, key).Replace(" ", "");
            return allFileType.Split(new[] { '|' }, StringSplitOptions.None);
        }

        public static List<string> GetTrimmedPipeSeparatedList(string xmlPath, string key)
        {
            List<string> values = new List<string>();
            foreach (string item in GetPipeSeparatedValues(xmlPath, key))
            {
                values.Add(item.Trim());
            }

            return values;
        }

        public static List<int> GetOneBasedNumericRangeFromValue(string xmlPath, string key)
        {
            List<int> values = new List<int>();
            string raw = GetValue(xmlPath, key);
            if (WordProcessCore.IsNum(raw))
            {
                int max = int.Parse(raw);
                for (int i = 1; i <= max; i++)
                {
                    values.Add(i);
                }
            }

            return values;
        }

        public static List<string> GetQuizCourseTypes(string xmlPath)
        {
            List<string> values = new List<string> { "全部显示" };
            values.AddRange(GetTrimmedPipeSeparatedList(xmlPath, "CourseType"));
            return values;
        }
    }
}
