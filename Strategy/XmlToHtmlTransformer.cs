using System;
using System.Xml.Xsl;

namespace DormitoryApp.Transform
{
    public class XmlToHtmlTransformer
    {
        public void TransformXmlToHtml(string xmlFilePath, string xslFilePath, string htmlOutputPath)
        {
            var xslt = new XslCompiledTransform();
            xslt.Load(xslFilePath);               // Завантажуємо XSLT
            xslt.Transform(xmlFilePath, htmlOutputPath); // Виконуємо трансформацію
        }
    }
}
