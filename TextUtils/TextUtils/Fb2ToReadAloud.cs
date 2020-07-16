using System;
using System.Collections.Generic;
using System.Text;

namespace Fb2ToReadAloudText
{
    public class Fb2ToReadAloud
    {
        public string FootnoteLeftDelimiterText 
        { 
            get => SnoskiConverter.FootnoteLeftDelimiterText;
            set => SnoskiConverter.FootnoteLeftDelimiterText = value;
        }
        public string FootnoteRightDelimiterText
        {
            get => SnoskiConverter.FootnoteRightDelimiterText;
            set => SnoskiConverter.FootnoteRightDelimiterText = value;
        }

        private Fb2ToTextConverter? _fb2ToTextConverter;
        internal Fb2ToTextConverter Fb2ToTextConverter 
        {
            get => _fb2ToTextConverter ??= new Fb2ToTextConverter(); 
            set => _fb2ToTextConverter = value; 
        }


        private SnoskiConverter? _snoskiConverter;
        internal SnoskiConverter SnoskiConverter
        {
            get => _snoskiConverter ??= new SnoskiConverter();
            set => _snoskiConverter = value;
        }

        public string ConvertToText(string fb2Xml)
        {
            string intermediateText = Fb2ToTextConverter.Convert(fb2Xml);
            return SnoskiConverter.Convert(intermediateText);
        }


    }
}
