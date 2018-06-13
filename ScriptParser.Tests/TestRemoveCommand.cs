using NUnit.Framework;
using System;
using System.IO;

namespace ScriptParser.Test
{
    [TestFixture]
    public class TestRemoveCommand: TestBase
    {
        [Test]
        public void TestSimple()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            const string text = "12345";
            File.WriteAllText(src, text);
            RemoveCommand rmc = new RemoveCommand(src);
            rmc.Execute();

            Assert.IsFalse(File.Exists(src));
        }

        [Test]
        public void TestInvalidSourceFilePath()
        {
            string src = TestConstants.TestDirectory + "/\0.txt";
            RemoveCommand rmc = new RemoveCommand(src);

            Assert.Throws<ArgumentException>(() => rmc.Execute());
        }

        [Test]
        public void TestNullSourceFilePath()
        {
            string src = null;
            RemoveCommand rmc = new RemoveCommand(src);

            Assert.Throws<ArgumentNullException>(() => rmc.Execute());
        }
    }
}

