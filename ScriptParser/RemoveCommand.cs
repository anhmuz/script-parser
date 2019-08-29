using System;
using System.IO;

namespace ScriptParser
{
    public class RemoveCommand: ICommand
    {
        private readonly string _source;
        private readonly Mode _mode;
        public enum Mode { File, Directory, Recursive };

        public RemoveCommand(string source, Mode mode = Mode.File)
        {
            _source = source;
            _mode = mode;
        }

        public CommandType Type
        {
            get { return CommandType.Remove; }
        }

        public string Info
        {
            get
            {
                switch (_mode)
                {
                    case Mode.File:
                        return string.Format("Deletes file {0}.", _source);
                    case Mode.Directory:
                        return string.Format(
                            "Deletes an empty directory from a path {0}.", _source);
                    case Mode.Recursive:
                        return string.Format(
                            "Deletes directory {0} and any subdirectories " +
                            "and files in this directory.", _source);
                }
                return "";
            }
        }

#pragma warning disable 0067
        public event Action<int> Progress;
        #pragma warning restore 0067

        public void Execute()
        {
            switch(_mode)
            {
            case Mode.File:
                try
                {
                    File.Delete(_source);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new IOException();
                }
                break;

            case Mode.Directory:
                try
                {
                    File.GetAttributes(_source);
                }
                catch(FileNotFoundException)
                {
                    break;
                }
                Directory.Delete(_source);
                break;

            case Mode.Recursive:
                if (Directory.Exists(_source))
                {
                    Directory.Delete(_source, true);
                }
                else
                {
                    File.Delete(_source);
                }
                break;
            }
        }
    }
}

