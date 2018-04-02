using System;
using System.IO;

namespace ScriptParser
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Unspecified path to script");
            }

            else if (args[0] == "--help" || args[0] == "-?")
            {
                Console.WriteLine("ScriptParser.exe <path to script>");
            }

            else if (args.Length > 1)
            {
                Console.WriteLine("Incorrect number of input arguments");
            }

            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("The file specified in path doesn't exist");
            }

            else
            {
                try
                {
                    ScriptParser sp = new ScriptParser();
                    sp.ParseScript(args[0]);
                    sp.ParsedScript.Execute();
                }
                catch (ScriptParserException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("line {0}, column {1}", e.line, e.column);
                }
            }
        }
    }
}
