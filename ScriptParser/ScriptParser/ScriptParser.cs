﻿using System;
using System.Collections.Generic;
using System.IO;

namespace ScriptParser
{
    public class ScriptParser
    {
        private Script _script = new Script();
        private string _source;
        private int _line;

        struct SizeMultiplier
        {
            public string Unit;
            public long Multiplier;

            public SizeMultiplier(string un, long mult)
            {
                Unit = un;
                Multiplier = mult;
            }
        }

        public Script ParsedScript
        {
            get { return _script; }
        }

        public CommandType ParseCommandType(string commandName)
        {
            switch (commandName)
            {
            case "copy":
                return CommandType.Copy;

            case "move":
                return CommandType.Move;

            case "remove":
                return CommandType.Remove;

            case "create_file":
                return CommandType.CreateFile;

            default:
                throw new ScriptParserException(
                    string.Format("unexpected command name: {0}", commandName),
                    _source, _line);
            }
        }

        public ICommand MakeCommand(CommandType commandType,
            List<string> arguments, string scriptPath)
        {
            switch (commandType)
            {
            case CommandType.Copy:
                if (arguments.Count == 2)
                {
                    return new CopyCommand(ParsePath(scriptPath, arguments[0]),
                        ParsePath(scriptPath, arguments[1]));
                }
                throw new ScriptParserException("copy expects 2 arguments",
                    _source, _line);

            case CommandType.Move:
                if (arguments.Count == 2)
                {
                    return new MoveCommand(ParsePath(scriptPath, arguments[0]),
                        ParsePath(scriptPath, arguments[1]));
                }
                throw new ScriptParserException("move expects 2 arguments",
                    _source, _line);

            case CommandType.Remove:
                if (arguments.Count == 1)
                {
                    return new RemoveCommand(
                        ParsePath(scriptPath, arguments[0]));
                }
                throw new ScriptParserException("remove expects 1 argument",
                    _source, _line);

            case CommandType.CreateFile:
                if (arguments.Count == 1)
                {
                    return new CreateFileCommand(
                        ParsePath(scriptPath,arguments[0]));
                }
                if (arguments.Count == 2)
                {
                    return new CreateFileCommand(
                        ParsePath(scriptPath,arguments[0]),
                        ParseFileSize(arguments[1]));
                }
                throw new ScriptParserException(
                    "create_file command expects 1 or 2 arguments",
                    _source, _line);

            default:
                throw new ScriptParserException("Undefined command type",
                    _source, _line);
            }
        }

        public List<string> ParseArguments(string text)
        {
            List<string> paths = new List<string>();
            int start = -1;
            bool quotes = false;

            for (int i = 0; i < text.Length; i++)
            {
                if (start == -1)
                {
                    if (text[i] == '"')
                    {
                        start = i + 1;
                        quotes = true;
                    }
                    else if (!char.IsWhiteSpace(text[i]))
                    {
                        start = i;
                        quotes = false;
                    }
                }
                else
                {
                    if (quotes && text[i] == '"')
                    {
                        if (i == text.Length - 1 ||
                            char.IsWhiteSpace(text[i + 1]))
                        {
                            paths.Add(text.Substring(start, i - start));
                            start = -1;
                            i++;
                        }
                        else
                        {
                            throw new ScriptParserException(
                                "Missing whitespace " +
                                "after closing double quote",
                            _source, _line, i);
                        }
                    }
                    else if (!quotes && char.IsWhiteSpace(text[i]))
                    {
                        paths.Add(text.Substring(start, i - start));
                        start = -1;
                    }
                }
            }

            if (start != -1)
            {
                if (quotes)
                {
                    throw new ScriptParserException(
                        "Last argument misses closing double quote",
                        _source, _line, text.Length);
                }
                paths.Add(text.Substring(start));
            }

            return paths;
        }

        public string ParsePath(string scriptPath, string argument)
        {
            string baseDir = Path.GetDirectoryName(scriptPath);
            if (!Path.IsPathRooted(argument))
            {
                argument = Path.Combine(baseDir, argument);
            }
            return argument;
		}

        public long ParseFileSize(string size)
        {
            SizeMultiplier[] multipliers = new SizeMultiplier[]
                {
                    new SizeMultiplier("MB", 1024 * 1024),
                    new SizeMultiplier("GB", 1024 * 1024 * 1024),
                    new SizeMultiplier("B", 1)
                };

            foreach (SizeMultiplier sm in multipliers)
            {
                if (size.EndsWith(sm.Unit))
                {
                    long result;
                    string s = size.Remove(size.Length - sm.Unit.Length);
                    if (long.TryParse(s, out result))
                    {
                        return result * sm.Multiplier;
                    }
                    throw new ScriptParserException(
                        String.Format("Invalid file size: {0}", size),
                        _source, _line);
                }
            }
            throw new ScriptParserException(
                String.Format("Invalid file size: {0}", size),
                _source, _line);
        }

        public void ParseScript(string path)
        {
            _source = path;
            string[] lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                _line = i;
                int index = lines[i].IndexOf(' ');
                string commandName = lines[i].Substring(0, index);
                CommandType commandType = ParseCommandType(commandName);
                List<string> arguments = ParseArguments(
                    lines[i].Substring(commandName.Length));
                ICommand command = MakeCommand(commandType, arguments, path);
                _script.AddCommand(command);
            }
        }
    }
}

