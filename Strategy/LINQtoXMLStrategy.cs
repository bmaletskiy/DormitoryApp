using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DormitoryApp.Models;

namespace DormitoryApp.Strategy
{
    public class LinqXmlSearchStrategy : IXmlSearchStrategy
    {
        public List<StudentInfo> Search(string filePath, string query)
        {
            var results = new List<StudentInfo>();
            var doc = XDocument.Load(filePath);

            var matches = from el in doc.Descendants("Student")
                          let name = el.Element("Name")?.Value ?? ""
                          where string.IsNullOrEmpty(query) || name.Contains(query, StringComparison.OrdinalIgnoreCase)
                          select new StudentInfo
                          {
                              Name = name,
                              Faculty = el.Attribute("faculty")?.Value ?? "",
                              Department = el.Attribute("department")?.Value ?? "",
                              Course = el.Element("Course")?.Value ?? "",
                              Room = el.Attribute("room")?.Value ?? "",
                              Bed = el.Attribute("bed")?.Value ?? "",
                              StartDate = el.Element("StartDate")?.Value ?? "",
                              EndDate = el.Element("EndDate")?.Value ?? ""
                          };

            results.AddRange(matches);
            return results;
        }
    }
}
