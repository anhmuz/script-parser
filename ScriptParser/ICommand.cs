using System;

namespace ScriptParser
{
    public interface ICommand
    {
        CommandType Type { get; }
        string Info { get; }

        void Execute();

        event Action<int> Progress;
    }
}

