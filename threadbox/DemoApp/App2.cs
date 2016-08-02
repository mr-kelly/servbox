using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadBoxLib;

namespace DemoApp
{
    public class App2 : IThread
    {
        protected override void OnStart(object arg)
        {
            while (true)
            {
                Console.WriteLine("From App2");
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}
