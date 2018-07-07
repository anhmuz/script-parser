﻿using System;
using System.IO;

namespace ScriptParser
{
    public class CopyCommand: ICommand
    {
        private readonly string _source;
        private readonly string _destination;

        public CopyCommand(string source, string destination)
        {
            _source = source;
            _destination = destination;
        }

        public CommandType Type
        {
            get { return CommandType.Copy; }
        }

        public event Action<int> Progress;

        public void Execute()
        {
            File.Copy(_source, _destination);
        }
    }
}

