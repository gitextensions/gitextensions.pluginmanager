﻿using GitExtensions.PluginManager.Properties;
using GitUIPluginInterfaces;
using PackageManager;
using ResourceManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GitExtensions.PluginManager
{
    /// <summary>
    /// A Git Extensions plugin for integrating PackageManager.
    /// </summary>
    [Export(typeof(IGitPlugin))]
    public class Plugin : GitPluginBase
    {
        public const string PackageId = @"GitExtensions.PluginManager";
        public const string GitExtensionsRelativePath = @"GitExtensions.exe";
        public const string PluginManagerRelativePath = @"PackageManager\PackageManager.UI.exe";
        public static readonly List<string> FrameworkMonikers = new List<string>() { "net5.0", "net6.0", "any", "netstandard2.0" };

        internal PluginSettings? Configuration { get; private set; }

        public Plugin()
            : base(PluginSettings.HasProperties)
        {
            Id = new Guid("9E2ECC51-AE38-4C73-9EA1-5D5D1D88022E");
            Name = "Plugin Manager";
            Description = "Plugin Manager";
            Icon = Resources.Icon;
        }

        public override void Register(IGitUICommands commands)
        {
            base.Register(commands);
            Configuration = new PluginSettings(Settings);
        }

        public override IEnumerable<ISetting> GetSettings()
            => Configuration!;

        public override bool Execute(GitUIEventArgs gitUiCommands)
        {
            string pluginsPath = ManagedExtensibility.UserPluginsPath!;
            
            Args args = new Args();
            args.Path = pluginsPath;
            args.Dependencies = new List<Args.Dependency>() { new Args.Dependency("GitExtensions.Extensibility", "0.3.0") };
            args.Tags = "GitExtensions";
            args.Monikers = FrameworkMonikers;
            args.SelfPackageId = PackageId;
            args.ProcessNamesToKillBeforeChange = new[] { Process.GetCurrentProcess().ProcessName };

            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = Path.Combine(pluginsPath, PackageId, PluginManagerRelativePath),
                Arguments = args.ToString(),
                UseShellExecute = false,
            };
            Process.Start(info);

            if (Configuration!.CloseInstances)
            {
                CloseAllOtherInstances();
                Application.Exit();
            }

            return false;
        }

        private void CloseAllOtherInstances()
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process other in Process.GetProcesses())
            {
                try
                {
                    if (other.MainModule != null && current.MainModule != null && other.MainModule.FileName == current.MainModule.FileName && other.Id != current.Id)
                        other.Kill();
                }
                catch (Win32Exception)
                {
                    continue;
                }
            }
        }
    }
}
