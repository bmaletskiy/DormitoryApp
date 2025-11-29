using DormitoryApp.Models;

namespace DormitoryApp.Strategy
{
    public class XmlSearchContext
    {
        private IXmlSearchStrategy strategy;

        public XmlSearchContext(IXmlSearchStrategy strategy)
        {
            this.strategy = strategy;
        }

        public void SetStrategy(IXmlSearchStrategy newStrategy)
        {
            strategy = newStrategy;
        }

        public List<StudentInfo> ExecuteSearch(
            string filePath,
            string nameQuery,
            string facultyQuery,
            string departmentQuery,
            string courseQuery)
        {
            return strategy.Search(
                filePath,
                nameQuery,
                facultyQuery,
                departmentQuery,
                courseQuery
            );
        }
    }
}
