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

        static string BasePath => Path.Combine(
                FileUtils.GetPathToCurrentAssemblyCsprojFolder(),
                    "Fb2ToTextConverterTestFiles");
        [Theory]
        [InlineData(@"FullXml\input.xml", @"FullXml\output.txt")]
        [InlineData(@"TitleTagsWrongLocation\input.xml", @"TitleTagsWrongLocation\output.txt")]
        [InlineData(@"NoTitleTags\input.xml", @"NoTitleTags\output.txt")]
        public void ConvertTest(string inputPath, string expectedOutputPath)
        {
            string input = File.ReadAllText(
                Path.Combine(BasePath, inputPath));

            var target = new Fb2ToTextConverter();
            string res = target.Convert(input);

            string expectedOutput = File.ReadAllText(
                Path.Combine(BasePath, expectedOutputPath));

            Assert.Equal(PrepareForCompare(expectedOutput),
                PrepareForCompare(res));
        }

        private string PrepareForCompare(string s)
        {
            return s.Replace("\r\n", "\n")
                .Trim('\n');
        }

        [Fact]
        public void Convert_NoBody_Exception()
        {
            string input = File.ReadAllText(
                Path.Combine(BasePath, @"NoBodyTags\input.xml"));

            var target = new Fb2ToTextConverter();
            Assert.Throws<Fb2BodyNotFound>(
                ()=>target.Convert(input));

        }
    }
}