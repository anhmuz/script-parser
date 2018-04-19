using System;

namespace ScriptParser
{
    public class ScriptParserException : Exception
    {
        public string errorSource;
        public int line;
        public int column;

        public ScriptParserException(string msg) : base(msg)
        {
        }

        public ScriptParserException(string msg, string source,
            int lineNumber, int positionInLine = -1) : base(msg)
        {
            errorSource = source;
            line = lineNumber;
            column = positionInLine;
        }
    }
}