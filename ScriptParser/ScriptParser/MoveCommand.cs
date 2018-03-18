using System;
using System.IO;

namespace ScriptParser
{
    public class MoveCommand: ICommand
    {
        private readonly string _source;
        private readonly string _destination;

        public MoveCommand(string source, string destination)
        {
            _source = source;
            _destination = destination;
        }

        public string CommandName
        {
            get { return "Move"; }
        }

        public void Execute()
        {
            File.Move(_source, _destination); 
        }
    }
}

