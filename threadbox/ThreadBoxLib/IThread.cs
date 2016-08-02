using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadBoxLib
{
    public abstract class IThread
    {
        public string DllPath { get; internal set; }
        public string ClassName { get; internal set; }
        public bool IsEnd { get; private set; }
        public void Start(object arg)
        {
            IsEnd = false;
            OnStart(arg);
            IsEnd = true;
        }

        protected abstract void OnStart(object arg);
    }
}
