using NUnit.Framework;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ScriptParser
{
    [TestFixture]
    public class TestCopyCommand
    {
        [SetUp]
        public void SetUp()
        {
            var testDirectory = new DirectoryInfo(TestConstants.TestDirectory);
            Array.ForEach(testDirectory.GetFiles(), file => file.Delete());
            Array.ForEach(testDirectory.GetDirectories(), directory => directory.Delete());
        }

        [Test]
        public void TestSimple()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            const string expected = "12345";
            File.WriteAllText(src, expected);
            CopyCommand cc = new CopyCommand(src, dst);
            cc.Execute();
            string actual = File.ReadAllText(dst);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMissingSourceFile()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            CopyCommand cc = new CopyCommand(src, dst);
            Assert.Throws<FileNotFoundException>(() => cc.Execute());
            Assert.IsFalse(File.Exists(dst));
        }

        [Test]
        public void TestInvalidSourceFilePath()
        {
            string src = TestConstants.TestDirectory + "/\0.txt";
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            CopyCommand cc = new CopyCommand(src, dst);
            Assert.Throws<ArgumentException>(() => cc.Execute());
            Assert.IsFalse(File.Exists(dst));
        }

        [Test]
        public void TestInvalidDestinationFilePath()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = TestConstants.TestDirectory + "/\0.txt";
            const string expected = "12345";
            File.WriteAllText(src, expected);
            CopyCommand cc = new CopyCommand(src, dst);
            Assert.Throws<ArgumentException>(() => cc.Execute());
            Assert.IsFalse(File.Exists(dst));
        }
    }

    public class TestConstants
    {
        public static string TestDirectory
        {
            get { return @"/home/anhelina/tmp"; }
        }
    }
}