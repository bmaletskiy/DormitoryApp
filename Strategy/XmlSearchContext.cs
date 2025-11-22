using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormitoryApp.Models; 

namespace DormitoryApp.Strategy
{
    public class XmlSearchContext
    {
        private IXmlSearchStrategy _strategy;

        public XmlSearchContext(IXmlSearchStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IXmlSearchStrategy strategy)
        {
            _strategy = strategy;
        }

        public List<StudentInfo> Search(string filePath, string query)
        {
            return _strategy.Search(filePath, query); 
        }
    }
}
