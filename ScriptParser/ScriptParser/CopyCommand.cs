using System;
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

        public string CommandName
        {
            get { return "Copy"; }
        }

        public void Execute()
        {
            File.Copy(_source, _destination); 
        }
    }
}

