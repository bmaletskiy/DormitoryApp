using System.Xml;
using DormitoryApp.Models;

namespace DormitoryApp.Strategy
{
    public class DomSearchStrategy : IXmlSearchStrategy
    {
        public List<StudentInfo> Search(
            string filePath,
            string nameQuery,
            string facultyQuery,
            string departmentQuery,
            string courseQuery)
        {
            var results = new List<StudentInfo>();
            var doc = new XmlDocument();
            doc.Load(filePath);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                //Ігноруємо всі непотрібні вузли
                if (node.NodeType != XmlNodeType.Element || node.Name != "Student")
                    continue;

                var student = new StudentInfo();

                // Атрибути
                student.Faculty = node.Attributes["faculty"]?.Value ?? "";
                student.Department = node.Attributes["department"]?.Value ?? "";
                student.Room = node.Attributes["room"]?.Value ?? "";
                student.Bed = node.Attributes["bed"]?.Value ?? "";

                // Дочірні елементи
                student.Name = node["Name"]?.InnerText ?? "";
                student.Course = node["Course"]?.InnerText ?? "";
                student.StartDate = node["StartDate"]?.InnerText ?? "";
                student.EndDate = node["EndDate"]?.InnerText ?? "";

                // Фільтрація
                bool matches =
                    (string.IsNullOrWhiteSpace(nameQuery) ||
                     (!string.IsNullOrEmpty(student.Name) &&
                      student.Name.Contains(nameQuery, StringComparison.OrdinalIgnoreCase))) &&

                    (string.IsNullOrWhiteSpace(facultyQuery) ||
                     (!string.IsNullOrEmpty(student.Faculty) &&
                      student.Faculty.Contains(facultyQuery, StringComparison.OrdinalIgnoreCase))) &&

                    (string.IsNullOrWhiteSpace(departmentQuery) ||
                     (!string.IsNullOrEmpty(student.Department) &&
                      student.Department.Contains(departmentQuery, StringComparison.OrdinalIgnoreCase))) &&

                    (string.IsNullOrWhiteSpace(courseQuery) ||
                     (!string.IsNullOrEmpty(student.Course) &&
                      student.Course.Equals(courseQuery, StringComparison.OrdinalIgnoreCase)));

                if (matches)
                    results.Add(student);
            }

            return results;
        }
    }
}
