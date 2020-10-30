using System;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace PackageManager
{
    public static class Program
    {
        private const string ApplicationKey = "5CFB158B-8346-4588-926D-99006A5195B6";
        private const int RestoreWindowCommandCode = 0x09;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [STAThread]
        public static void Main()
        {
            using var mutex = new Mutex(false, ApplicationKey, out bool createdNew);

            if (createdNew)
            {
                var application = new App();

                application.InitializeComponent();
                application.Run();
            }
            else
            {
                var currentProcess = Process.GetCurrentProcess();
                var mainProcess = Process.GetProcesses()
                    .Where(x => x.Id != currentProcess.Id)
                    .FirstOrDefault(x => x.ProcessName == currentProcess.ProcessName);

                if (mainProcess != null)
                {
                    ShowWindow(mainProcess.MainWindowHandle, RestoreWindowCommandCode);
                    SetForegroundWindow(mainProcess.MainWindowHandle);
                }
            }
        }
    }
}
