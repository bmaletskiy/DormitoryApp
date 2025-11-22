using System;
using System.Collections.Generic;
using System.Xml;
using DormitoryApp.Models;

namespace DormitoryApp.Strategy
{
    public class DomSearchStrategy : IXmlSearchStrategy
    {
        public List<StudentInfo> Search(string filePath, string query)
        {
            var results = new List<StudentInfo>();
            var doc = new XmlDocument();
            doc.Load(filePath);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes) // node = <Student>
            {
                var student = new StudentInfo();

                if (node.Attributes != null)
                {
                    student.Faculty = node.Attributes["faculty"]?.Value ?? "";
                    student.Department = node.Attributes["department"]?.Value ?? "";
                    student.Room = node.Attributes["room"]?.Value ?? "";
                    student.Bed = node.Attributes["bed"]?.Value ?? "";
                }

                // Читаємо дочірні елементи
                student.Name = node["Name"]?.InnerText ?? "";
                student.Course = node["Course"]?.InnerText ?? "";
                student.StartDate = node["StartDate"]?.InnerText ?? "";
                student.EndDate = node["EndDate"]?.InnerText ?? "";

                // Фільтр по запиту (Name або частковий збіг)
                if (string.IsNullOrEmpty(query) || student.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                    results.Add(student);
            }

            return results;
        }
    }
}
