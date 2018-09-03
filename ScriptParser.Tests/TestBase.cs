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
            if (Directory.Exists(TestConstants.TestDirectory))
            {
                Directory.Delete(TestConstants.TestDirectory, true);
            }
            Directory.CreateDirectory(TestConstants.TestDirectory);
        }
    }
}

