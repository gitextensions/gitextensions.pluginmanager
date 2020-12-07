using Neptuo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    partial class ProcessService
    {
        internal class ProcessKillContext
        {
            private readonly IReadOnlyCollection<Process> targets;

            public ProcessKillContext(IReadOnlyCollection<string> names)
            {
                Ensure.NotNull(names, "names");
                ProcessNames = names;
                targets = names.SelectMany(name => Process.GetProcessesByName(name)).ToList();
            }

            public IReadOnlyCollection<string> ProcessNames { get; }
            
            public int ProcessCount { get => targets.Count; }

            public bool IsExecutable => targets.Count > 0;

            public void Execute()
            {
                foreach (Process process in targets)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Win32Exception)
                    {
                        continue;
                    }
                }
            }
        }
    }
}
