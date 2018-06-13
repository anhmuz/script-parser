using NUnit.Framework;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Mono.Unix.Native;

namespace ScriptParser.Test
{
    [TestFixture]
    public class TestCopyCommand: TestBase
    {
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

        [Test]
        public void TestNullSourceFilePath()
        {
            string src = null;
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            CopyCommand cc = new CopyCommand(src, dst);

            Assert.Throws<ArgumentNullException>(() => cc.Execute());
            Assert.IsFalse(File.Exists(dst));
        }

        [Test]
        public void TestNullDestinationFilePath()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = null;
            const string expected = "12345";
            File.WriteAllText(src, expected);
            CopyCommand cc = new CopyCommand(src, dst);

            Assert.Throws<ArgumentNullException>(() => cc.Execute());
            Assert.IsFalse(File.Exists(dst));
        }

        [Test]
        public void TestExistingDestinationFile()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            const string expected = "12345";
            File.WriteAllText(src, expected);
            File.Create(dst);
            CopyCommand cc = new CopyCommand(src, dst);

            Assert.Throws<IOException>(() => cc.Execute());
            Assert.IsTrue(File.Exists(dst));
        }

        [Test]
        public void TestBadPermissions()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            const string expected = "12345";
            File.WriteAllText(src, expected);
            CopyCommand cc = new CopyCommand(src, dst);

            #if (__MonoCS__)
                Syscall.chmod(src, FilePermissions.S_IWUSR);
            #else
                // TODO
            #endif
            Assert.Throws<UnauthorizedAccessException>(() => cc.Execute());
        }
    }
}