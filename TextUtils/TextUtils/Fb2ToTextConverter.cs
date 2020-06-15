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

            var nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("m", "http://www.gribuser.ru/xml/fictionbook/2.0");  //иначе не XPath теги не видит

            var author = doc.SelectSingleNode("/m:FictionBook/m:description/m:title-info/m:author", nsMgr);


            throw new NotImplementedException();    //**!!

            //var root = XElement.Parse(fb2Xml);
            //var author = root.XPathSelectElement("/FictionBook");
            //string authorName = $"{author.Element("last-name")} {author.Element("first-name")} {author.Element("middle-name")}"
            //    .Trim();

            
        }
    }
}
