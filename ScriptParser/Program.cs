using System;
using System.IO;

namespace ScriptParser
{
    class Program
    {
        private static void PrintUsage()
        {
            Console.WriteLine("Usage: ScriptParser.exe <path to script>");
            Console.WriteLine(
                "Execute script specified in <path to script>");
        }

        private static bool ValidateArguments(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ScriptParser.exe: missing script path");
                return false;
            }
            else if (args.Length > 1)
            {
                Console.WriteLine("Incorrect number of input arguments");
                return false;
            }
            return true;
        }

        static void PrintProgress(int progress)
        {
            Console.WriteLine("Executed {0} %", progress);
        }

        public static int Main(string[] args)
        {
            if (!ValidateArguments(args))
            {
                Console.WriteLine(
                    "Try 'ScriptParser.exe --help' for more information.");
                return -1;
            }

            if (args[0] == "--help" || args[0] == "-?")
            {
                PrintUsage();
                return 0;
            }


            if (!File.Exists(args[0]))
            {
                Console.WriteLine(String.Format(
                    "ScriptParser.exe: failed to access '{0}': No such file",
                    args[0]));
                return -1;
            }

            try
            {
                ScriptParser sp = new ScriptParser();
                sp.ParseScript(args[0]);
                sp.ParsedScript.Progress += PrintProgress;
                sp.ParsedScript.Execute();
                return 0;
            }
            catch (ScriptParserException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("path: {0} line: {1} column: {2}",
                    e.errorSource, e.line, e.column);
                return -1;
            }
        }
    }
}
