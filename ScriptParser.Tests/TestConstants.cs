using System;
using System.Reflection;
using System.IO;

namespace ScriptParser.Test
{
    public class TestConstants
    {
        public static string TestDirectory
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location), "tmp");
            }
        }
    }
}

