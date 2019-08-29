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

        public CommandType Type
        {
            get { return CommandType.Move; }
        }

        public string Info
        {
            get
            {
                var info = string.Format("Moves file {0} to a new location: {1}",
                    _source, _destination);
                return info;
            }
        }

#pragma warning disable 0067
        public event Action<int> Progress;
        #pragma warning restore 0067

        public void Execute()
        {
            File.Move(_source, _destination); 
        }
    }
}

