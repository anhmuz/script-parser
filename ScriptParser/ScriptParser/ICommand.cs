using System;

namespace ScriptParser
{
    public interface ICommand
    {
        ScriptParser.CommandType Type { get; }

        void Execute();
    }
}

