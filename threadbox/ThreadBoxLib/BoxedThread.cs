using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadBoxLib
{
    public abstract class BoxedThread
    {
        public string DllPath { get; internal set; }
        public string ClassName { get; internal set; }
        public bool IsEnd { get; private set; }
        public Thread Thread { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }

        public void Start(object arg)
        {
            IsEnd = false;
            OnStart(arg);
            IsEnd = true;
        }

        protected abstract void OnStart(object arg);

        public void Stop()
        {
            Thread.Abort();
            IsEnd = true;
        }
    }
}
