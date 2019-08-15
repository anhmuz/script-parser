using NUnit.Framework;
using System;
using System.Threading;
using System.IO;

namespace ScriptParser.Test
{
    [TestFixture]
    class TestScript: TestBase
    {
        [Test]
        public void TestCancelSleep()
        {
            var sc = new SleepCommand(TimeSpan.FromMilliseconds(10000));
            var script = new Script();
            script.AddCommand(sc);
            OperationCanceledException exception = null;

            Thread scriptThread = new Thread(() =>
            {
                try
                {
                    script.Execute();
                }
                catch (OperationCanceledException e)
                {
                    exception = e;
                }
            });
            scriptThread.Start();
            script.Cancel();
            scriptThread.Join();
            Assert.IsNotNull(exception);
        }

        [Test]
        public void TestCancelCopy()
        {
            string src = Path.Combine(TestConstants.TestDirectory, "a.txt");
            string dst = Path.Combine(TestConstants.TestDirectory, "b.txt");
            File.WriteAllText(src, "12345");
            var cc = new CopyCommand(src, dst);

            var script = new Script();
            script.AddCommand(cc);

            script.Progress += ((progress) =>
            {
                if (progress > 20)
                {
                    script.Cancel();
                }
            });
            Assert.Throws<OperationCanceledException>(() => script.Execute());
        }
    }
}
