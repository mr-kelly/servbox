using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadBoxLib
{
    class ThreadBoxStarter
    {
        /// <summary>
        /// Main thread always sleep, until waiter reset
        /// </summary>
        public static readonly AutoResetEvent ThreadWaiter = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var dllPath = Path.GetFullPath("DemoApp.dll");
            ThreadBox.StartThread(dllPath, "DemoApp.App1");
            ThreadBox.StartThread(dllPath, "DemoApp.App2");
            ThreadPool.QueueUserWorkItem((_) =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(5000);
                    ThreadBox.StartThread(dllPath, "DemoApp.App1");
                }
            });


            while (true)
            {
                ThreadWaiter.WaitOne(Timeout.Infinite);
            }
        }
    }
}
