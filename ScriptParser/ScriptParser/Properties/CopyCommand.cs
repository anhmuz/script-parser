using System;
using System.IO;
using System.Linq;

namespace ScriptParser
{
    public class CopyCommand: ICommand
    {
        private string _source;
        private string _destination;

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

