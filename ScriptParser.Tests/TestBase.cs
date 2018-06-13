using NUnit.Framework;
using System;
using System.IO;

namespace ScriptParser.Test
{
    public class TestBase
    {
        [SetUp]
        public void SetUp()
        {
            var testDirectory = new DirectoryInfo(TestConstants.TestDirectory);
            Array.ForEach(testDirectory.GetFiles(), file => file.Delete());
            Array.ForEach(testDirectory.GetDirectories(),
                directory => directory.Delete());
        }
    }
}

