using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ThreadBoxLib
{
    public class ThreadBox
    {
        static List<IThread> _threads = new List<IThread>(); 
        /// <summary>
        /// Star a thread by arguments
        /// </summary>
        /// <param name="dllFullPath"></param>
        /// <param name="typeName"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static IThread StartThread(string dllFullPath, string typeName, object arg = null)
        {
            var asm = Assembly.Load(File.ReadAllBytes(dllFullPath));
            var threadType = asm.GetType(typeName);
            IThread thread = (IThread)Activator.CreateInstance(threadType);
            thread.DllPath = dllFullPath;
            thread.ClassName = typeName;
            lock (_threads)
            {
                _threads.Add(thread);
            }
            ThreadPool.QueueUserWorkItem(thread.Start, arg);
            return thread;
        }

        /// <summary>
        /// Remove useless, ended threads
        /// </summary>
        public static void Clean()
        {
            lock (_threads)
            {
                for (var i = _threads.Count - 1; i >= 0; i--)
                {
                    var thread = _threads[i];
                    if (thread.IsEnd)
                        _threads.RemoveAt(i);
                }
                
            }
        }
    }
}
