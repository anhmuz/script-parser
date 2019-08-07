using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptParser.Test
{
    [TestFixture]
    class TestSleepCommand: TestBase
    {
        [Test]
        public void TestSimple()
        {
            SleepCommand sc = new SleepCommand(10000);
            var start = DateTime.Now;
            sc.Execute();
            TimeSpan span = DateTime.Now - start;

            Assert.IsFalse((int)span.TotalMilliseconds < 10000);
        }
    }
}
