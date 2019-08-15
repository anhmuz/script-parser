using System;
using System.Collections.Generic;

namespace ScriptParser
{
    public class Script: ICommand
    {
        private List<ICommand> _commands = new List<ICommand>();
        int? _previousProgress;
        int _executedCommandCount;
        bool _cancel = false;

        public CommandType Type
        {
            get { return CommandType.Execute; }
        }

        public void AddCommand(ICommand c)
        {
            _commands.Add(c);
        }

        public event Action<int> Progress;

        private void ReportProgress(int progress)
        {
            if (_cancel)
            {
                throw new OperationCanceledException("Script execution is canceled");
            }
            if (Progress == null)
            {
                return;
            }
            int currentProgress =
                (_executedCommandCount * 100 + progress) / _commands.Count;
            if (_previousProgress != currentProgress)
            {
                Progress(currentProgress);
                _previousProgress = currentProgress;
            }
        }

        public void Cancel()
        {
            _cancel = true;
        }

        public void Execute()
        {
            foreach (ICommand c in _commands)
            {
                ReportProgress(0);
                c.Progress += ReportProgress;
                c.Execute();
                c.Progress -= ReportProgress;
                ReportProgress(100);
                _executedCommandCount++;
            }
        }
    }
}

