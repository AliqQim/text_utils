using Xunit;
using Fb2ToReadAloudText;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CommonUtils;

namespace Fb2ToReadAloudText.Tests
{
    public class Fb2ToTextConverterTests
    {
        [Theory]
        [InlineData(@"FullXml\input.xml", @"FullXml\output.txt")]
        [InlineData(@"TitleTagsWrongLocation\input.xml", @"TitleTagsWrongLocation\output.txt")]
        [InlineData(@"NoTitleTags\input.xml", @"NoTitleTags\output.txt")]
        public void ConvertTest(string inputPath, string expectedOutputPath)
        {
            string basePath = Path.Combine(
                FileUtils.GetPathToCurrentAssemblyCsprojFolder(),
                    "Fb2ToTextConverterTestFiles");
            string input = File.ReadAllText(
                Path.Combine(basePath, inputPath));

            var target = new Fb2ToTextConverter();
            string res = target.Convert(input);

            string expectedOutput = File.ReadAllText(
                Path.Combine(basePath, expectedOutputPath));

            Assert.Equal(PrepareForCompare(expectedOutput),
                PrepareForCompare(res));
        }

        private string PrepareForCompare(string s)
        {
            return s.Replace("\r\n", "\n")
                .Trim('\n');
        }
    }
}