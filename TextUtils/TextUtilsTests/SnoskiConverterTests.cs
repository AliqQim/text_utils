using Xunit;
using snoski;
using System;
using System.Collections.Generic;
using System.Text;
using CommonUtils;
using System.IO;
using System.Collections;
using System.Linq;

namespace snoski.Tests
{
    public class SnoskiConverterTests
    {
        class SnoskiTestCasesProvider : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                return Directory.GetDirectories(GetPathToTestCasesFolders())
                    .Select(x => new[] { new FileInfo(x).Name }).GetEnumerator();

            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }

        public static string GetPathToTestCasesFolders()
        {
            string pathToProj = FileUtils.GetPathToCurrentAssemblyCsprojFolder();
            return $"{pathToProj}\\SnoskiConverterTestFiles";
        }

        [Theory]
        [ClassData(typeof(SnoskiTestCasesProvider))]
        public void ConvertTest(string testFolderName)
        {
            string pathToProj = FileUtils.GetPathToCurrentAssemblyCsprojFolder();
            string testFolderPath = Path.Combine(GetPathToTestCasesFolders(), testFolderName);


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