using System;
using NUnit.Framework;

namespace ScriptParser.Test
{
    [TestFixture]
    class TestSleepCommand: TestBase
    {
        [Test]
        public void TestSimple()
        {
            SleepCommand sc = new SleepCommand(TimeSpan.FromMilliseconds(10000));
            var start = DateTime.Now;
            sc.Execute();
            TimeSpan span = DateTime.Now - start;

            Assert.That((int)span.TotalMilliseconds, Is.GreaterThanOrEqualTo(10000));
        }
    }
}
