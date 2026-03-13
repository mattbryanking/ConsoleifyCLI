using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;
using Microsoft.Win32;
using System.Diagnostics;

namespace ConsoleifyCLI.ConfigTasks
{
    public class AutoHideTaskbarTask : IInstallOption
    {
        public int Id => 2;
        public string Name => "Set Taskbar to auto-hide";
        public string Category => "Configuration";
        public bool IsSelected { get; set; } = false;
        public bool HasWarning => true;
        public bool IsRevertSupported => true;

        // windows taskbar settings binary blob
        private const string RegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\StuckRects3";

        public Task ExecuteAsync()
        {
            if (UpdateTaskbarRegistry(true))
            {
                RestartExplorer();
                ConsoleHelper.Success("Taskbar set to auto-hide!");
            }
            return Task.CompletedTask;
        }

        public Task RevertAsync()
        {
            if (UpdateTaskbarRegistry(false))
            {
                RestartExplorer();
                ConsoleHelper.Success("Taskbar returned to default always-on-top state.");
            }
            return Task.CompletedTask;
        }

        // this is a scary ass method! working on Windows 11 Home OS Build 26200.7840
        private bool UpdateTaskbarRegistry(bool autoHide)
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
            {
                if (key != null)
                {
                    byte[]? settings = (byte[]?)key.GetValue("Settings");

                    // taskbar settings blob (StuckRect3) is usually 48 bytes
                    if (settings != null && settings.Length >= 9)
                    {
                        // we want index 8 for the autohide flag
                        settings[8] = (byte)(autoHide ? 0x03 : 0x02);

                        key.SetValue("Settings", settings, RegistryValueKind.Binary);
                        return true;
                    }
                    else
                    {
                        ConsoleHelper.Error("Taskbar settings blob is missing or a non-standard size. Aborting.");
                        return false;
                    }
                }
            }
            ConsoleHelper.Error("Failed to open registry key for taskbar settings. Aborting.");
            return false;
        }

        private void RestartExplorer()
        {
            ConsoleHelper.Warning("Restarting Windows Explorer (your screen will flash black for a moment)...");

            var explorerProcesses = Process.GetProcessesByName("explorer");
            foreach (var process in explorerProcesses)
            {
                process.Kill();
                process.WaitForExit(5000); 
            }
        }
    }
}