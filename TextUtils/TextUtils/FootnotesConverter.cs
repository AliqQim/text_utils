using Fb2ToReadAloudText;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Fb2ToReadAloudText
{
    //исторически сложилось, что этот класс умеет оперировать уже текстом, 
    //произведенным из FB2. поэтому в текст, принимаемый этим классом FB2 
    //преобразуется снаружи
    internal class FootnotesConverter
    {
        public string FootnoteLeftDelimiterText { get; set; } = "**";
        public string FootnoteRightDelimiterText { get; set; } = "**";

        static Regex _number = new Regex(@"^\s*(\d+)\s*$", RegexOptions.Compiled);
        const int NUMBER_GROUP_INDEX = 1;

        class FootnoteInfo
        {
            public FootnoteInfo(int num, string text)
            {
                Num = num;
                Text = text;
            }

            public int Num { get; set; }
            public string Text { get; set; }
            public bool Used { get; set; }
        }

        List<FootnoteInfo>? _footnoteInfo;

       

        public string Convert(string input)
        {

            List<string> lines = ConvertToLines(input);


            List<string> footnoteTextLines = new List<string>();    //текст сноски (потом заджойним)
            int? lastFootnoteLineIndex = null;
            int? lastFootnoteNum = null;

            _footnoteInfo = new List<FootnoteInfo>();
            

            for (int lineBackwardsIterator = lines.Count - 1; lineBackwardsIterator > 0; lineBackwardsIterator--)
            {
                string line = lines[lineBackwardsIterator];

                var matchRes = _number.Match(line);
                if (matchRes.Success)
                {
                    int thisFootnoteNum = int.Parse(matchRes.Groups[NUMBER_GROUP_INDEX].Value);

                    //если номер не бьется - значит, уже не сноски
                    if (lastFootnoteNum != null &&
                        (lastFootnoteNum.Value <= thisFootnoteNum || //предыдуще считанная сноска должна быть больше этой
                        _footnoteInfo.Select(x => x.Num).Contains(thisFootnoteNum)//и не должна повторяться
                        ))
                        break;

                    lastFootnoteLineIndex = lineBackwardsIterator;

                    footnoteTextLines.Reverse();
                    _footnoteInfo.Add(new FootnoteInfo(
                        num: thisFootnoteNum,
                        text: string.Join(" ", footnoteTextLines.Where(x => !string.IsNullOrWhiteSpace(x)))
                    ));

                    footnoteTextLines.Clear();

                    lastFootnoteNum = thisFootnoteNum;

                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        footnoteTextLines.Add(line);
                    }
                }

            }

            int upperBorderOfMainText = lastFootnoteLineIndex != null ? lastFootnoteLineIndex.Value - 1 : lines.Count - 1;

            //считали сноски, теперь формируем, собственно, основной текст
            var linesOfMainText = lines.GetRange(0, upperBorderOfMainText + 1);


            //теперь прописываем в основной текст текст сносок
            var finalTextLines = new List<string>();    //OPTIMIZE
            foreach (string line in linesOfMainText)
            {
                finalTextLines.Add(ReplaceFootnote(line));
           
            }

            var unuseds = _footnoteInfo.Where(x => !x.Used);
            if (unuseds.Any())
            {
                throw new UnusedFootnoteFoundException($"Обнаружены неиспользуемые сноски: {string.Join(", ", unuseds.Select(x => x.Num))}");
            }

            return string.Join("\r\n", finalTextLines);

        }

        private List<string> ConvertToLines(string input)
        {
            var lines = new List<string>();

            using (StringReader reader = new StringReader(input))
            {

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }

        private string ReplaceFootnote(string input)
        {
            string result = input;
            while (true)
            {
                var match = Regex.Match(result, @"\[\s*(\d+)\s*\]");
                if (!match.Success)
                    break;

                int numOfFootnote = int.Parse(match.Groups[1].Value);
                string footnoteText = $" {FootnoteLeftDelimiterText}{UseFootnoteText(numOfFootnote)}{FootnoteRightDelimiterText}";

                result = result.Substring(0, match.Index) + footnoteText + result.Substring(match.Index + match.Length);
            }

            return result;
        }

        private string UseFootnoteText(int footnoteNum)
        {
            try
            {
                var FootnoteInfo = _footnoteInfo.Single(x => x.Num == footnoteNum);
                FootnoteInfo.Used = true;
                return FootnoteInfo.Text;
            }
            catch (InvalidOperationException e)
            {
                throw new FootnoteNotFoundException($"сноска не обнаружена: {footnoteNum}", e);
            }
            
        }
    }
}