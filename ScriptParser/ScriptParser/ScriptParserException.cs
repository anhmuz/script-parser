using System;

namespace ScriptParser
{
    public class ScriptParserException : Exception
    {
        public int line;
        public int column;

        public ScriptParserException(string msg) : base(msg)
        {
        }

        public ScriptParserException(string msg,
            int lineNumber, int positionInLine = -1) : base(msg)
        {
            line = lineNumber;
            column = positionInLine;
        }
    }
}