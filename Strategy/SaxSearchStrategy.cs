using System;
using System.Collections.Generic;
using System.Xml;
using DormitoryApp.Models;

namespace DormitoryApp.Strategy
{
    public class SaxSearchStrategy : IXmlSearchStrategy
    {
        public List<StudentInfo> Search(string filePath, string query)
        {
            var results = new List<StudentInfo>();

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                StudentInfo student = null;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Student")
                    {
                        student = new StudentInfo
                        {
                            Faculty = reader.GetAttribute("faculty") ?? "",
                            Department = reader.GetAttribute("department") ?? "",
                            Room = reader.GetAttribute("room") ?? "",
                            Bed = reader.GetAttribute("bed") ?? ""
                        };
                    }
                    else if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (student == null) continue;

                        switch (reader.Name)
                        {
                            case "Name":
                                student.Name = reader.ReadElementContentAsString();
                                break;
                            case "Course":
                                student.Course = reader.ReadElementContentAsString();
                                break;
                            case "StartDate":
                                student.StartDate = reader.ReadElementContentAsString();
                                break;
                            case "EndDate":
                                student.EndDate = reader.ReadElementContentAsString();
                                break;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Student")
                    {
                        if (student != null && (string.IsNullOrEmpty(query) || student.Name.Contains(query, StringComparison.OrdinalIgnoreCase)))
                        {
                            results.Add(student);
                            student = null;
                        }
                    }
                }
            }

            return results;
        }
    }
}
