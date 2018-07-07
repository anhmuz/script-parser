using System;
using System.IO;

namespace ScriptParser
{
    public class CreateFileCommand: ICommand
    {
        static readonly byte[] _pageData = null; 
        static readonly int _pageSize = 4 * 1024;
        private readonly string _source;
        private readonly long _size;

        static CreateFileCommand()
        {
            int dataSize = _pageSize + 9;
            _pageData = new byte[dataSize];
            for (int i = 0; i < _pageData.Length; i++)
            {
                _pageData[i] = (byte)(i % 10);
            }
        }

        public CreateFileCommand(string source)
        {
            _source = source;
        }

        public CreateFileCommand(string source, long size)
        {
            _source = source;
            _size = size;
        }

        public CommandType Type
        {
            get { return CommandType.CreateFile; }
        }

        public event Action<int> Progress;

        public void Execute()
        {
            using (FileStream file = File.Create(_source))
            {
                int pageNumber = (int)(_size / _pageSize);
                if (_size % _pageSize > 0)
                {
                    pageNumber =+ 1;
                }
                for (int i = 0; i < pageNumber; i++)
                {
                    int currentPageSize;
                    if (i == pageNumber - 1 && _size % _pageSize > 0)
                    {
                        currentPageSize = (int)(_size % _pageSize);
                    }
                    else
                    {
                        currentPageSize = _pageSize;
                    }

                    int offset = (_pageSize * i) % 10;
                    file.Write(_pageData, offset, currentPageSize);
                }
            }
        }
    }
}

