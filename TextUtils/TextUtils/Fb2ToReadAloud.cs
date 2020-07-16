using System;
using System.Collections.Generic;
using System.Text;

namespace Fb2ToReadAloudText
{
    public class Fb2ToReadAloud
    {
        public string FootnoteLeftDelimiterText 
        { 
            get => FootnoteConverter.FootnoteLeftDelimiterText;
            set => FootnoteConverter.FootnoteLeftDelimiterText = value;
        }
        public string FootnoteRightDelimiterText
        {
            get => FootnoteConverter.FootnoteRightDelimiterText;
            set => FootnoteConverter.FootnoteRightDelimiterText = value;
        }

        private Fb2ToTextConverter? _fb2ToTextConverter;
        internal Fb2ToTextConverter Fb2ToTextConverter 
        {
            get => _fb2ToTextConverter ??= new Fb2ToTextConverter(); 
            set => _fb2ToTextConverter = value; 
        }


        private FootnotesConverter? _footnotesConverter;
        internal FootnotesConverter FootnoteConverter
        {
            get => _footnotesConverter ??= new FootnotesConverter();
            set => _footnotesConverter = value;
        }

        public string ConvertToText(string fb2Xml)
        {
            string intermediateText = Fb2ToTextConverter.Convert(fb2Xml);
            return FootnoteConverter.Convert(intermediateText);
        }


    }
}
