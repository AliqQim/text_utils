using Xunit;
using Fb2ToReadAloudTextTests;
using System;
using System.Collections.Generic;
using System.Text;
using CommonUtils;
using System.IO;
using System.Collections;
using System.Linq;
using Fb2ToReadAloudText;

namespace Fb2ToReadAloudTextTests.Tests
{
    public class FootnoteConverterTests
    {
        class FootnoteTestCasesProvider : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                return Directory.GetDirectories(GetPathToPositiveTestCasesFolders())
                    .Select(x => new[] { new FileInfo(x).Name }).GetEnumerator();

            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }

        public static string GetPathToExceptionTestCasesFolders()
            => Path.Combine(GetPathToTestCasesFolders(), "exceptions");
        public static string GetPathToPositiveTestCasesFolders()
            => Path.Combine(GetPathToTestCasesFolders(), "positives");
        public static string GetPathToTestCasesFolders()
        {
            string pathToProj = FileUtils.GetPathToCurrentAssemblyCsprojFolder();
            return $"{pathToProj}\\Footnotes" +
                $"ConverterTestFiles";
        }

        [Theory]
        [ClassData(typeof(FootnoteTestCasesProvider))]
        public void ConvertPositiveTestcases(string testFolderName)
        {
            string testFolderPath = Path.Combine(GetPathToPositiveTestCasesFolders(), testFolderName);


            string input = File.ReadAllText(Path.Combine(testFolderPath, "input.txt"));
            var target = new FootnotesConverter();
            var res = target.Convert(input);

            Assert.Equal(
                File.ReadAllText(Path.Combine(testFolderPath, "output.txt")),
                res);
        }

        [Theory]
        [InlineData("Convert_НеиспользованнаяСноска_Исключение", typeof(UnusedFootnoteFoundException))]
        [InlineData("Convert_СноскаНеНайдена_Исключение", typeof(FootnoteNotFoundException))]
        public void Convert_NegativeTestcases(string testFolderName, Type expectedException)
        {
            string input = File.ReadAllText(
                Path.Combine(GetPathToExceptionTestCasesFolders(), testFolderName, "input.txt"));

            var target = new FootnotesConverter();
            Assert.Throws(expectedException, () => target.Convert(input));

        }
    }
}