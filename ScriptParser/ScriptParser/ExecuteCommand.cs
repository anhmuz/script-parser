using System;
using System.IO;

namespace ScriptParser
{
    public class ExecuteCommand: ICommand
    {
        private readonly Script _script;

        public ExecuteCommand(Script script)
        {
            _script = script;
        }

        public CommandType Type
        {
            get { return CommandType.Execute; }
        }

        public void Execute()
        {
            _script.Execute();
        }
    }
}
