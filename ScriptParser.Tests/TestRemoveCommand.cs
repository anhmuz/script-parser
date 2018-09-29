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

        [Test]
        public void TestEmptyDirectoryRemoval()
        {
            string emptyDirSource =
                Path.Combine(TestConstants.TestDirectory, "a");
            Directory.CreateDirectory(emptyDirSource);
            RemoveCommand rmc = new RemoveCommand(emptyDirSource,
                    RemoveCommand.Mode.Directory);
            rmc.Execute();

            Assert.IsFalse(Directory.Exists(emptyDirSource));
        }

        [Test]
        public void TestDirectoryRecursiveRemoval()
        {
            string dirSource = Path.Combine(TestConstants.TestDirectory, "a/b");
            Directory.CreateDirectory(dirSource);
            RemoveCommand rmc = new RemoveCommand(dirSource,
                RemoveCommand.Mode.Recursive);
            rmc.Execute();

            Assert.IsFalse(Directory.Exists(dirSource));
        }

        [Test]
        public void TestFileRecursiveRemoval()
        {
            string fileSource = Path.Combine(TestConstants.TestDirectory, "a");
            const string text = "12345";
            File.WriteAllText(fileSource, text);
            RemoveCommand rmc = new RemoveCommand(fileSource,
                RemoveCommand.Mode.Recursive);
            rmc.Execute();

            Assert.IsFalse(File.Exists(fileSource));
        }

        [Test]
        public void TestDirectoryWrongModeRemoval()
        {
            string dirSource = Path.Combine(TestConstants.TestDirectory, "a");
            Directory.CreateDirectory(Path.Combine(dirSource, "b"));
            RemoveCommand rmc = new RemoveCommand(dirSource,
                RemoveCommand.Mode.Directory);

            Assert.Throws<IOException>(() => rmc.Execute());
        }

        [Test]
        public void TestNonexistentFileRemoval()
        {
            string fileSource = Path.Combine(TestConstants.TestDirectory, "a");
            RemoveCommand rmc = new RemoveCommand(fileSource);

            Assert.DoesNotThrow(() => rmc.Execute());
        }

        [Test]
        public void TestNonexistentDirectoryRemoval()
        {
            string dirSource = Path.Combine(TestConstants.TestDirectory, "a");
            RemoveCommand rmc1 = new RemoveCommand(dirSource,
                RemoveCommand.Mode.Directory);
            RemoveCommand rmc2 = new RemoveCommand(dirSource,
                RemoveCommand.Mode.Recursive);

            Assert.DoesNotThrow(() => rmc1.Execute());
            Assert.DoesNotThrow(() => rmc2.Execute());
        }

        [Test]
        public void TestWrongModeRemoval()
        {
            string dirSource = Path.Combine(TestConstants.TestDirectory, "a");
            Directory.CreateDirectory(dirSource);
            string fileSource = Path.Combine(TestConstants.TestDirectory,"b");
            const string text = "12345";
            File.WriteAllText(fileSource, text);
            RemoveCommand rmc1 = new RemoveCommand(fileSource,
                RemoveCommand.Mode.Directory);
            RemoveCommand rmc2 = new RemoveCommand(dirSource,
                RemoveCommand.Mode.File);

            Assert.Throws<IOException>(() => rmc1.Execute());
            Assert.Throws<IOException>(() => rmc2.Execute());
        }
    }
}

