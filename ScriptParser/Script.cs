using System;
using System.Collections.Generic;

namespace ScriptParser
{
    public class Script: ICommand
    {
        private List<ICommand> commands = new List<ICommand>();

        public CommandType Type
        {
            get { return CommandType.Execute; }
        }

        public void AddCommand(ICommand c)
        {
            commands.Add(c);
        }

        public void Execute()
        {
            foreach (ICommand c in commands)
            {
                c.Execute();
            }
        }
    }
}

