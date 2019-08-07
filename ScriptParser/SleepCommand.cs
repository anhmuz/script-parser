using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptParser
{
    public class SleepCommand: ICommand
    {
        private int _sleepingTime;

        public SleepCommand(int sleepingTime)
        {
            _sleepingTime = sleepingTime;
        }

        public CommandType Type
        {
            get { return CommandType.Sleep; }
        }

        public event Action<int> Progress;

        public void Execute()
        {
            if (Progress != null)
            {
                Progress(0);
            }
            var start = DateTime.Now;
            while (true)
            {
                Thread.Sleep(1000);
                TimeSpan span = DateTime.Now - start;
                if (Progress != null)
                {
                    Progress((int)span.TotalMilliseconds * 100 / _sleepingTime);
                }
                if ((int)span.TotalMilliseconds >= _sleepingTime)
                {
                    break;
                }
            }
        }

    }
}
