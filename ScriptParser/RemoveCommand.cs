using System;
using System.IO;

namespace ScriptParser
{
    public class RemoveCommand: ICommand
    {
        private readonly string _source;

        public RemoveCommand(string source)
        {
            _source = source;
        }

        public CommandType Type
        {
            get { return CommandType.Remove; }
        }

        public event Action<int> Progress;

        public void Execute()
        {
            File.Delete(_source); 
        }
    }
}

