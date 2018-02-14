using System;

namespace ScriptParser
{
    public interface ICommand
    {
        string CommandName { get; }
        void Execute();
    }
}

