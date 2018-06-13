﻿using System;

namespace ScriptParser
{
    public interface ICommand
    {
        CommandType Type { get; }

        void Execute();
    }
}
