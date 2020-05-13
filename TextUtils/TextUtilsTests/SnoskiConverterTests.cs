using Xunit;
using snoski;
using System;
using System.Collections.Generic;
using System.Text;
using CommonUtils;
using System.IO;

namespace snoski.Tests
{
    public class SnoskiConverterTests
    {
        [Theory]
        [InlineData("Convert_ведущийпробелПередНомеромСноски_Заменено")]
        public void ConvertTest(string testFolderName)
        {
            string pathToProj = FileUtils.GetPathToCurrentAssemblyCsprojFolder();
            string testFolderPath = $"{pathToProj}\\SnoskiConverterTestFiles\\{testFolderName}";


            string input = File.ReadAllText(Path.Combine(testFolderPath, "input.txt"));
            var target = new SnoskiConverter();
            var res = target.Convert(input);

            Assert.Equal(
                File.ReadAllText(Path.Combine(testFolderPath, "output.txt")),
                res
                );
        }
    }
}