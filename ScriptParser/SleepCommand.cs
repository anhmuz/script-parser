using System;
using System.Threading;

namespace ScriptParser
{
    public class SleepCommand: ICommand
    {
        private TimeSpan _sleepInterval;

        public SleepCommand(TimeSpan sleepInterval)
        {
            _sleepInterval = sleepInterval;
        }

        public CommandType Type
        {
            get { return CommandType.Sleep; }
        }

        public event Action<int> Progress;

        public void Execute()
        {
            Progress?.Invoke(0);
            var start = DateTime.Now;
            var end = start + _sleepInterval;
            var stepInterval = new TimeSpan(_sleepInterval.Ticks / 100);
            while (true)
            {
                var now = DateTime.Now;
                Thread.Sleep(now + stepInterval > end ? end - now : stepInterval);

                var elapsed = DateTime.Now - start;
                var progress = (int)(elapsed.Ticks * 100 / _sleepInterval.Ticks);
                Progress?.Invoke(progress);
                if (elapsed >= _sleepInterval)
                {
                    break;
                }
            }
        }

    }
}
