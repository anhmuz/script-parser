using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScriptParser.Test
{
    [TestFixture]
    public class TestScriptParser: TestBase
    {
        [Test]
        public void TestParseArguments()
        {
            ScriptParser sp = new ScriptParser();

            Assert.AreEqual(new List<string> { "a", "a b" },
                sp.ParseArguments("a \"a b\""));
            
            Assert.AreEqual(new List<string> { "a b", "a" },
                sp.ParseArguments("\"a b\" a"));
        }

        [Test]
        public void TestWhiteSpaces()
        {
            ScriptParser sp = new ScriptParser();

            Assert.AreEqual(new List<string> { "a b", "a" },
                sp.ParseArguments(" \"a b\"  a "));
        }

        [Test]
        public void TestMissingWhiteSpace()
        {
            ScriptParser sp = new ScriptParser();

            Assert.AreEqual(new List<string> { "a\"b\"" },
                sp.ParseArguments("a\"b\""));
        }

        [Test]
        public void TestMissingQuote()
        {
            ScriptParser sp = new ScriptParser();

            Assert.Throws<ScriptParserException>(
                () => sp.ParseArguments("\"a b\" \"a"));
        }

        [Test]
        public void TestMissingWhiteSpaceAfterClosingQuote()
        {
            ScriptParser sp = new ScriptParser();

            Assert.Throws<ScriptParserException>(
                () => sp.ParseArguments("\"b\"a"));
        }

        [Test]
        public void TestMakeCopyCommand()
        {
            ScriptParser sp = new ScriptParser();

            Assert.IsInstanceOf(typeof(CopyCommand),
                sp.MakeCommand(CommandType.Copy,
                    new List<string> { "a", "b" }, "fake_path"));

            Assert.Throws<ScriptParserException>(
                () => sp.MakeCommand(CommandType.Copy,
                    new List<string> { "a" }, "fake_path"));
        }

        [Test]
        public void TestMakeMoveCommand()
        {
            ScriptParser sp = new ScriptParser();

            Assert.IsInstanceOf(typeof(MoveCommand),
                sp.MakeCommand(CommandType.Move,
                    new List<string> { "a", "b" }, "fake_path"));

            Assert.Throws<ScriptParserException>(
                () => sp.MakeCommand(CommandType.Move,
                    new List<string> { "a" }, "fake_path"));
        }

        [Test]
        public void TestMakeRemoveCommand()
        {
            ScriptParser sp = new ScriptParser();

            Assert.IsInstanceOf(typeof(RemoveCommand),
                sp.MakeCommand(CommandType.Remove,
                    new List<string> { "a" }, "fake_path"));

            Assert.Throws<ScriptParserException>(
                () => sp.MakeCommand(CommandType.Remove,
                    new List<string> { "a", "b" }, "fake_path"));
        }

        [Test]
        public void TestMakeExecuteCommand()
        {
            string callerPath = Path.Combine(
                TestConstants.TestDirectory, "caller");

            string calleePath = Path.Combine(
                TestConstants.TestDirectory,"callee");
            File.WriteAllText(calleePath, "");

            string invalidScriptPath = Path.Combine(
                TestConstants.TestDirectory,"invalidScript");
            File.WriteAllText(invalidScriptPath, "create_file");

            ScriptParser sp = new ScriptParser();

            Assert.IsInstanceOf(typeof(Script),
                sp.MakeCommand(CommandType.Execute,
                    new List<string> { "callee" }, callerPath));

            Assert.Throws<ScriptParserException>(
                () => sp.MakeCommand(CommandType.Execute,
                    new List<string> { "callee", "badArgument" }, callerPath));
            Assert.Throws<FileNotFoundException>(
                () => sp.MakeCommand(CommandType.Execute,
                    new List<string> { "fake" }, callerPath));
            Assert.Throws<ScriptParserException>(
                () => sp.MakeCommand(CommandType.Execute,
                    new List<string> { "invalidScript" }, callerPath));
        }

        [Test]
        public void TestParseFileSize()
        {
            ScriptParser sp = new ScriptParser();
            long tmp = 25L * 1024 * 1024 * 1024;
            Assert.AreEqual(25, sp.ParseFileSize("25B"));
            Assert.AreEqual(25 * 1024 * 1024, sp.ParseFileSize("25MB"));
            Assert.AreEqual(tmp, sp.ParseFileSize("25GB"));

            Assert.Throws<ScriptParserException>(() => sp.ParseFileSize("25"));
            Assert.Throws<ScriptParserException>(() => sp.ParseFileSize("aaB"));
        }
    }
}

