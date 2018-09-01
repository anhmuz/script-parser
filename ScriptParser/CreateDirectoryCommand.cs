using System;
using System.IO;

namespace ScriptParser
{
    public class CreateDirectoryCommand: ICommand
    {
        private readonly string _source;

        public CreateDirectoryCommand(string source)
        {
            _source = source;
        }

        public CommandType Type
        {
            get { return CommandType.CreateDirectory; }
        }

        #pragma warning disable 0067
        public event Action<int> Progress;
        #pragma warning restore 0067

        public void Execute()
        {
            Directory.CreateDirectory(_source);
        }
    }
}

