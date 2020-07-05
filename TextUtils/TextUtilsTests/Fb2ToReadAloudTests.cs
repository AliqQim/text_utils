using CommonUtils;
using Fb2ToReadAloudText;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Fb2ToReadAloudText.Tests
{

    public class Fb2ToReadAloudTests
    {
        [Theory]
        [InlineData(@"Fb2ToReadAloudTests\ConvertToTextTest\vech_zhizn_smerti.fb2",
            @"Fb2ToReadAloudTests\ConvertToTextTest\expectedOutput.txt")]
        public void ConvertToTextTest(string fb2Path, string expectedOutputPath)
        {
            var target = new Fb2ToReadAloud();
            string res = target.ConvertToText(
                File.ReadAllText(
                    Path.Combine(
                        FileUtils.GetPathToCurrentAssemblyCsprojFolder(),
                        fb2Path)));

            string expectedRes = File.ReadAllText(
                    Path.Combine(
                        FileUtils.GetPathToCurrentAssemblyCsprojFolder(),
                        expectedOutputPath));

        }
    }
}