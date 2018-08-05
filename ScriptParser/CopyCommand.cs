using System;
using System.IO;

namespace ScriptParser
{
    public class CopyCommand: ICommand
    {
        private class FileCopyHelper: IDisposable
        {
            static readonly int _pageSize = 4 * 1024;
            private FileStream _sourceStream;
            private FileStream _destinationStream;
            byte[] _buffer = new byte[_pageSize];

            public int PageNumber
            {
                get
                {
                    int pageNumber = (int)(_sourceStream.Length / _pageSize);
                    if (_sourceStream.Length % _pageSize > 0)
                    {
                        pageNumber += 1;
                    }
                    return pageNumber;
                }

            }

            public FileCopyHelper(string src, string dst)
            {
                _sourceStream = new FileStream(
                    src, FileMode.Open, FileAccess.Read);
                try
                {
                    _destinationStream = new FileStream(
                    dst, FileMode.Create, FileAccess.Write);
                }
                catch(Exception)
                {
                    _sourceStream.Dispose();
                    throw;
                }
            }

            public void CopyPage()
            {
                int n = _sourceStream.Read(_buffer, 0, _pageSize);
                _destinationStream.Write(_buffer, 0, n);
            }

            public void Dispose()
            {
                _sourceStream.Dispose();
                _destinationStream.Dispose();
            }
        }
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
            using (var fch = new FileCopyHelper(_source, _destination))
            {
                for (int i = 0; i < fch.PageNumber; i++)
                {
                    fch.CopyPage();
                    if (Progress != null)
                    {
                        Progress((i + 1) * 100 / fch.PageNumber);
                    }
                }
            }
        }
    }
}

