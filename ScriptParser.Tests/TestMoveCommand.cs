using NUnit.Framework;
using System;
using System.IO;

namespace ScriptParser.Test
{
    [TestFixture]
    public class TestMoveCommand: TestBase
    {
        [Test]
        public void TestSimple()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            const string expected = "12345";
            File.WriteAllText(src, expected);
            MoveCommand mc = new MoveCommand(src, dst);
            mc.Execute();
            string actual = File.ReadAllText(dst);

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(File.Exists(src));
        }

        [Test]
        public void TestMissingSourceFile()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            MoveCommand mc = new MoveCommand(src, dst);

            Assert.Throws<FileNotFoundException>(() => mc.Execute());
            Assert.IsFalse(File.Exists(src));
            Assert.IsFalse(File.Exists(dst));
        }

        [Test]
        public void TestExistingDestinationFile()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            const string expected = "12345";
            File.WriteAllText(src, expected);
            File.Create(dst).Dispose();
            MoveCommand mc = new MoveCommand(src, dst);

            Assert.Throws<IOException>(() => mc.Execute());
            Assert.IsTrue(File.Exists(src));
            Assert.IsTrue(File.Exists(dst));
        }
    }
}

