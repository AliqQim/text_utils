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
        [Fact()]
        public void ConvertTest()
        {
            string inputFileName = "input_fullXml.xml";

            string input = File.ReadAllText(
                Path.Combine(
                    FileUtils.GetPathToCurrentAssemblyCsprojFolder(),
                    "Fb2ToTextConverterTest",
                    inputFileName)
                );

            var target = new Fb2ToTextConverter();
            string res = target.Convert(input);

            Assert.True(false, "This test needs an implementation");
        }
    }
}