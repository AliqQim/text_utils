using CommonUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Fb2ToReadAloudText
{
    internal class Fb2ToTextConverter
    {
        public string Convert(string fb2Xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(fb2Xml);

            var nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("m", "http://www.gribuser.ru/xml/fictionbook/2.0");  //иначе не XPath теги не видит

            var author = doc.SelectSingleNode("/m:FictionBook/m:description/m:title-info/m:author", nsMgr);
            string? authorName = null;
            if (author != null)
            {
                authorName = $"{author.SelectSingleNode("m:last-name", nsMgr)?.InnerText} " +
                    $"{author.SelectSingleNode("m:first-name", nsMgr)?.InnerText} " +
                    $"{author.SelectSingleNode("m:middle-name", nsMgr)?.InnerText}"
                    .Trim();
            }
            
            string? bookTitle = doc.SelectSingleNode("/m:FictionBook/m:description/m:title-info/m:book-title", nsMgr)
                ?.InnerText;

            var bodies = doc.SelectNodes("/m:FictionBook/m:body", nsMgr);

            if (bodies.Count == 0)
                throw new Fb2BodyNotFound();

            //OPTIMIZE можно какой-нибудь стринг-билдер использовать
            string bodiesTextRaw = string.Join("\n", bodies.OfType<XmlNode>().Select(n => BodyToText(n)));
            string bodiesTextNoLongParagraphs = RemoveLongParagraph(bodiesTextRaw);

            return $"{bookTitle}\n\n" +
                $"{authorName}\n\n" +
                bodiesTextNoLongParagraphs;

        }

        private string BodyToText(XmlNode body)
        {
            return TextUtils.StripTags(body.InnerXml);
        }


        private string RemoveLongParagraph(string bodiesTextRaw)
        {
            //IMPROVE: по идее, можно потом наумничать алгоритм, разбивающий реально только большие абзацы
            return bodiesTextRaw.Replace(". ", ".\n");
        }
    }
}
