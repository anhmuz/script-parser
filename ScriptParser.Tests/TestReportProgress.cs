using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;

namespace ScriptParser.Test
{
    [TestFixture()]
    public class TestReportProgress: TestBase
    {
        private List<int> _progressValues = new List<int>();

        private void AddProgressValue(int value)
        {
            _progressValues.Add(value);
        }

        [Test()]
        public void TestSimple()
        {
            string scriptPath = Path.Combine(
                TestConstants.TestDirectory,"script");
            File.WriteAllText(scriptPath,
                @"create_file a
copy a b
remove a
execute script1");

            string script1Path = Path.Combine(
                TestConstants.TestDirectory,"script1");
            File.WriteAllText(script1Path, "copy b c");

            ScriptParser sp = new ScriptParser();
            sp.ParseScript(scriptPath);
            sp.ParsedScript.Progress += AddProgressValue;
            sp.ParsedScript.Execute();

            Assert.AreEqual(new List<int> { 0, 25, 50, 75, 100 },
                _progressValues);
        }
    }
}

