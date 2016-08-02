using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ThreadBoxLib
{
    /// <summary>
    /// watch file
    /// </summary>
    class DllWatcher
    {
        /// <summary>
        /// dll path -> DllWatcher
        /// </summary>
        public static Dictionary<string, DllWatcher> _watchers = new Dictionary<string, DllWatcher>();
        public static Dictionary<string, FileSystemWatcher> _filewatchers = new Dictionary<string, FileSystemWatcher>();
        //static List<string> _dllList = new List<string>();
        public string DllFullPath;
        DllWatcher(string dllFullPath, FileSystemEventHandler handler)
        {
            DllFullPath = dllFullPath;

            var dirPath = Path.GetDirectoryName(Path.GetFullPath(dllFullPath));

            FileSystemWatcher watcher;
            if (!_filewatchers.TryGetValue(dirPath, out watcher))
            {
                _filewatchers[dirPath] = watcher = new FileSystemWatcher();

                watcher.Path = dirPath;
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.EnableRaisingEvents = true;
                watcher.Changed += (sender, args) =>
                {
                    if (args.FullPath == DllFullPath)
                    {
                        Console.WriteLine("Changed!: " + dllFullPath);
                        handler(sender, args);
                    }
                };
            }
        }
        public static DllWatcher Get(string dllPath, FileSystemEventHandler handler)
        {
            DllWatcher dllwatcher;
            var dllFullPath = Path.GetFullPath(dllPath);
            if (!_watchers.TryGetValue(dllFullPath, out dllwatcher))
            {
                dllwatcher = _watchers[dllFullPath] = new DllWatcher(dllFullPath, handler);
            }
            return dllwatcher;


        }
    }
}
