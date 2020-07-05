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
    public class SnoskiConverter
    {

        static Regex _number = new Regex(@"^\s*(\d+)\s*$", RegexOptions.Compiled);
        const int NUMBER_GROUP_INDEX = 1;

        class SnoskaInfo
        {
            public SnoskaInfo(int num, string text)
            {
                Num = num;
                Text = text;
            }

            public int Num { get; set; }
            public string Text { get; set; }
            public bool Used { get; set; }
        }

        List<SnoskaInfo>? _snoskaInfo;

        public string Convert(string input)
        {

            List<string> lines = ConvertToLines(input);


            List<string> snoskaTextLines = new List<string>();    //текст сноски (потом заджойним)
            int? lastSnoskaLineIndex = null;
            int? lastSnoskaNum = null;

            _snoskaInfo = new List<SnoskaInfo>();
            

            for (int lineBackwardsIterator = lines.Count - 1; lineBackwardsIterator > 0; lineBackwardsIterator--)
            {
                string line = lines[lineBackwardsIterator];

                var matchRes = _number.Match(line);
                if (matchRes.Success)
                {
                    int thisSnoskaNum = int.Parse(matchRes.Groups[NUMBER_GROUP_INDEX].Value);

                    //если номер не бьется - значит, уже не сноски
                    if (lastSnoskaNum != null &&
                        (lastSnoskaNum.Value <= thisSnoskaNum || //предыдуще считанная сноска должна быть больше этой
                        _snoskaInfo.Select(x => x.Num).Contains(thisSnoskaNum)//и не должна повторяться
                        ))
                        break;

                    lastSnoskaLineIndex = lineBackwardsIterator;

                    snoskaTextLines.Reverse();
                    _snoskaInfo.Add(new SnoskaInfo(
                        num: thisSnoskaNum,
                        text: string.Join(" ", snoskaTextLines.Where(x => !string.IsNullOrWhiteSpace(x)))
                    ));

                    snoskaTextLines.Clear();

                    lastSnoskaNum = thisSnoskaNum;

                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        snoskaTextLines.Add(line);
                    }
                }

            }

            int upperBorderOfMainText = lastSnoskaLineIndex != null ? lastSnoskaLineIndex.Value - 1 : lines.Count - 1;

            //считали сноски, теперь формируем, собственно, основной текст
            var linesOfMainText = lines.GetRange(0, upperBorderOfMainText + 1);


            //теперь прописываем в основной текст текст сносок
            var finalTextLines = new List<string>();    //OPTIMIZE
            foreach (string line in linesOfMainText)
            {
                finalTextLines.Add(ReplaceSnoskas(line));
           
            }

            var unuseds = _snoskaInfo.Where(x => !x.Used);
            if (unuseds.Any())
            {
                throw new UnusedSnoskaFoundException($"Обнаружены неиспользуемые сноски: {string.Join(", ", unuseds.Select(x => x.Num))}");
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

        private string ReplaceSnoskas(string input)
        {
            string result = input;
            while (true)
            {
                var match = Regex.Match(result, @"\[\s*(\d+)\s*\]");
                if (!match.Success)
                    break;

                int numOfSnoska = int.Parse(match.Groups[1].Value);
                string snoskaText = $" **{UseSnoskaText(numOfSnoska)}**";

                result = result.Substring(0, match.Index) + snoskaText + result.Substring(match.Index + match.Length);
            }

            return result;
        }

        private string UseSnoskaText(int snoskaNum)
        {
            try
            {
                var snoskaInfo = _snoskaInfo.Single(x => x.Num == snoskaNum);
                snoskaInfo.Used = true;
                return snoskaInfo.Text;
            }
            catch (InvalidOperationException e)
            {
                throw new SnoskaNotFoundException($"сноска не обнаружена: {snoskaNum}", e);
            }
            
        }
    }
}