using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;

namespace ScriptParser.Test
{
    [TestFixture()]
    public class TestReportProgress: TestBase
    {
        [Test()]
        public void TestSimple()
        {
            Test("script", new List<Tuple<string, string>> {
                new Tuple<string, string> ("script",
                    @"create_file a
copy a b
remove a
execute script1") ,
                new Tuple<string, string> ("script1", "copy b c")},
                new List<int> { 0, 25, 50, 75, 100 });
        }

        [Test()]
        public void TestCreateFile()
        {
            Test(@"create_file a 18KB", new List<int> { 0, 20, 40, 60, 80, 100 });
        }

        [Test()]
        public void TestCopyFile()
        {
            Test(@"create_file a 5KB
copy a b", new List<int> { 0, 25, 50, 75, 100 });
        }

        private void Test(string script, List<int> expectedProgress)
        {
            Test("script", new List<Tuple<string, string>> { new Tuple<string, string>("script", script) }, expectedProgress);
        }

        private void Test(string entryScriptName,
            List<Tuple<string, string>> scripts,
            List<int> expectedProgress)
        {
            foreach (var script in scripts)
            {
                string scriptPath = Path.Combine(
                    TestConstants.TestDirectory, script.Item1);
                File.WriteAllText(scriptPath, script.Item2);
            }

            ScriptParser sp = new ScriptParser();
            sp.ParseScript(Path.Combine(
                TestConstants.TestDirectory, entryScriptName));
            List<int> progressValues = new List<int>();
            Action<int> progressHandler =
                (int progress) => progressValues.Add(progress);
            sp.ParsedScript.Progress += progressHandler;
            sp.ParsedScript.Execute();
            sp.ParsedScript.Progress -= progressHandler;

            Assert.AreEqual(expectedProgress, progressValues);
        }
    }
}

