using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Fb2ToReadAloudText
{
    public class Fb2ToTextConverter
    {
        public string Convert(string fb2Xml)
        {

            var doc = new XmlDocument();
            doc.LoadXml(fb2Xml);
            var author = doc.SelectSingleNode("/FictionBook/description/title-info/author");

            //var root = XElement.Parse(fb2Xml);
            //var author = root.XPathSelectElement("/FictionBook");
            //string authorName = $"{author.Element("last-name")} {author.Element("first-name")} {author.Element("middle-name")}"
            //    .Trim();

            throw new NotImplementedException();    //**!!
        }
    }
}
