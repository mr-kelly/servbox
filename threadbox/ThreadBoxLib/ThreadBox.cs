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
        private static volatile int _threadIdCounter = 0;
        /// <summary>
        /// 启动过的线程储存在此
        /// </summary>
        static List<BoxedThread> _threads = new List<BoxedThread>();

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// Star a thread by arguments
        /// </summary>
        /// <param name="dllPath"></param>
        /// <param name="typeName"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static BoxedThread StartThread(string name, string dllPath, string typeName, object arg = null, bool autoReload = false)
        {
            lock (Locker)
            {
                var asm = Assembly.Load(File.ReadAllBytes(dllPath));
                var threadType = asm.GetType(typeName);
                BoxedThread boxedThread = (BoxedThread)Activator.CreateInstance(threadType);
                boxedThread.Name = name;
                boxedThread.DllPath = dllPath;
                boxedThread.ClassName = typeName;
                boxedThread.Id = _threadIdCounter++;

                Console.WriteLine(string.Format("Start thread: `{0}`, dll: `{1}`, class: `{2}`, id: `{3}`", name, dllPath, typeName, boxedThread.Id));
                lock (_threads)
                {
                    _threads.Add(boxedThread);
                }
                Thread thread = new Thread(boxedThread.Start);
                boxedThread.Thread = thread;
                thread.Start();

//                DllWatcher.Get(dllPath, (sender, args) =>
//                {
//                    Console.WriteLine("Abort thread: " + name);
//                    boxedThread.Stop();
//                    StartThread(name, dllPath, typeName);
//                });

                return boxedThread;
            }
        }

        /// <summary>
        /// 清理已经结束了的线程
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

        /// <summary>
        /// 读取Ini文件信息并单进程启动所有thread
        /// </summary>
        public static void StartAllWithIni(string iniFile = null)
        {
            if (iniFile == null)
                iniFile = "threadbox.ini";// default ini path

            var iniParser = new IniParser.FileIniDataParser();
            var iniData = iniParser.ReadFile(iniFile);
            foreach (var section in iniData.Sections)
            {
                var threadPrefxi = "thread:";
                if (section.SectionName.StartsWith(threadPrefxi))
                {
                    var threadName = section.SectionName.Substring(threadPrefxi.Length, section.SectionName.Length - threadPrefxi.Length);
                    var threadDllPath = iniData[section.SectionName]["dll_path"];
                    var threadClass = iniData[section.SectionName]["class"];
                    ThreadBox.StartThread(threadName, threadDllPath, threadClass);
                }
            }
        }
    }
}
