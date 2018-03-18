using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ScriptParser.Test
{
    [TestFixture]
    public class TestScriptParser
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
                sp.MakeCommand(ScriptParser.CommandType.Copy,
                    new List<string> { "a", "b" }));

            Assert.Throws<ScriptParserException>(
                () => sp.MakeCommand(ScriptParser.CommandType.Copy,
                    new List<string> { "a" }));
        }

        [Test]
        public void TestMakeMoveCommand()
        {
            ScriptParser sp = new ScriptParser();

            Assert.IsInstanceOf(typeof(MoveCommand),
                sp.MakeCommand(ScriptParser.CommandType.Move,
                    new List<string> { "a", "b" }));

            Assert.Throws<ScriptParserException>(
                () => sp.MakeCommand(ScriptParser.CommandType.Move,
                    new List<string> { "a" }));
        }

        [Test]
        public void TestMakeRemoveCommand()
        {
            ScriptParser sp = new ScriptParser();

            Assert.IsInstanceOf(typeof(RemoveCommand),
                sp.MakeCommand(ScriptParser.CommandType.Remove,
                    new List<string> { "a" }));

            Assert.Throws<ScriptParserException>(
                () => sp.MakeCommand(ScriptParser.CommandType.Remove,
                    new List<string> { "a", "b" }));
        }
    }
}

