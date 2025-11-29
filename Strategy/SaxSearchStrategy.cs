using System.Xml;
using DormitoryApp.Models;

namespace DormitoryApp.Strategy
{
    public class SaxSearchStrategy : IXmlSearchStrategy
    {
        public List<StudentInfo> Search(
            string filePath,
            string nameQuery,
            string facultyQuery,
            string departmentQuery,
            string courseQuery)
        {
            var results = new List<StudentInfo>();

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                StudentInfo? student = null;

                while (reader.Read())
                {
                    // Початок вузла Student
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

                    // Дочірні елементи
                    else if (reader.NodeType == XmlNodeType.Element && student != null)
                    {
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

                    // Кінець Student — перевірка фільтрів
                    else if (reader.NodeType == XmlNodeType.EndElement &&
                             reader.Name == "Student" &&
                             student != null)
                    {
                        bool matches =
                            (string.IsNullOrWhiteSpace(nameQuery) ||
                             (student.Name != null &&
                              student.Name.Contains(nameQuery, StringComparison.OrdinalIgnoreCase))) &&

                            (string.IsNullOrWhiteSpace(facultyQuery) ||
                             (student.Faculty != null &&
                              student.Faculty.Contains(facultyQuery, StringComparison.OrdinalIgnoreCase))) &&

                            (string.IsNullOrWhiteSpace(departmentQuery) ||
                             (student.Department != null &&
                              student.Department.Contains(departmentQuery, StringComparison.OrdinalIgnoreCase))) &&

                            (string.IsNullOrWhiteSpace(courseQuery) ||
                             (student.Course != null &&
                              student.Course.Equals(courseQuery, StringComparison.OrdinalIgnoreCase)));

                        if (matches)
                            results.Add(student);

                        student = null;
                    }
                }
            }

            return results;
        }
    }
}
