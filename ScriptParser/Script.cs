using System;
using System.Collections.Generic;

namespace ScriptParser
{
    public class Script
    {
        private List<ICommand> commands = new List<ICommand>();

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

