using System;
using System.IO;

namespace ScriptParser
{
    public class CopyCommand: ICommand
    {
        static readonly int _pageSize = 4 * 1024;
        private readonly string _source;
        private readonly string _destination;

        public CopyCommand(string source, string destination)
        {
            _source = source;
            _destination = destination;
        }

        public CommandType Type
        {
            get { return CommandType.Copy; }
        }

        public event Action<int> Progress;

        public void Execute()
        {
            if (Progress != null)
            {
                Progress(0);
            }
            using (FileStream source = new FileStream(
                _source, FileMode.Open, FileAccess.Read))
            using (FileStream destination = new FileStream(
                _destination, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = new byte[_pageSize];
                int readBytes = 0;
                int pageNumber = (int)(source.Length / _pageSize);
                if (source.Length % _pageSize > 0)
                {
                    pageNumber += 1;
                }
                for (int i = 0; i < pageNumber; i++)
                {
                    int n = source.Read(bytes, 0, _pageSize);
                    destination.Write(bytes, 0, n);
                    readBytes += n;
                    if (Progress != null)
                    {
                        Progress((i + 1) * 100 / pageNumber);
                    }
                }
            }
        }
    }
}

