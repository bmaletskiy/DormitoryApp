using DormitoryApp.Models;

namespace DormitoryApp.Strategy
{
    public interface IXmlSearchStrategy
    {
        List<StudentInfo> Search(string filePath, string query);
    }
}
