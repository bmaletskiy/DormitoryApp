using System.Xml.Linq;
using DormitoryApp.Models;

namespace DormitoryApp.Strategy
{
    public class LinqXmlSearchStrategy : IXmlSearchStrategy
    {
        public List<StudentInfo> Search(
            string filePath,
            string nameQuery,
            string facultyQuery,
            string departmentQuery,
            string courseQuery)
        {
            var doc = XDocument.Load(filePath);

            var students =
                from el in doc.Descendants("Student")
                let name = el.Element("Name")?.Value ?? ""
                let faculty = el.Attribute("faculty")?.Value ?? ""
                let department = el.Attribute("department")?.Value ?? ""
                let course = el.Element("Course")?.Value ?? ""
                where
                    (string.IsNullOrWhiteSpace(nameQuery) ||
                     (name != null && name.Contains(nameQuery, StringComparison.OrdinalIgnoreCase))) &&

                    (string.IsNullOrWhiteSpace(facultyQuery) ||
                     (faculty != null && faculty.Contains(facultyQuery, StringComparison.OrdinalIgnoreCase))) &&

                    (string.IsNullOrWhiteSpace(departmentQuery) ||
                     (department != null && department.Contains(departmentQuery, StringComparison.OrdinalIgnoreCase))) &&

                    (string.IsNullOrWhiteSpace(courseQuery) ||
                     (course != null && course.Equals(courseQuery, StringComparison.OrdinalIgnoreCase)))

                select new StudentInfo
                {
                    Name = name,
                    Faculty = faculty,
                    Department = department,
                    Course = course,
                    Room = el.Attribute("room")?.Value ?? "",
                    Bed = el.Attribute("bed")?.Value ?? "",
                    StartDate = el.Element("StartDate")?.Value ?? "",
                    EndDate = el.Element("EndDate")?.Value ?? ""
                };

            return students.ToList();
        }
    }
}
