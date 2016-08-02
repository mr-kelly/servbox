using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadBoxLib;

namespace DemoApp
{
    public class App1 : IThread 
    {
        protected override void OnStart(object arg)
        {
            while (true)
            {
                Console.WriteLine("from app1 hot");
                System.Threading.Thread.Sleep(300);
            }
        }
    }

}
