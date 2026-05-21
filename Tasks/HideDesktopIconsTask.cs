using System.Runtime.InteropServices;
using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;
using Microsoft.Win32;

namespace ConsoleifyCLI.Tasks
{
    public class HideDesktopIconsTask : IInstallOption
    {
        public string Name => "Hide desktop icons";
        public string Category => "Configuration";
        public bool IsSelected { get; set; } = false;
        public bool HasWarning => false;
        public bool IsUninstallSupported => true;

        private const string RegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string RegistryValue = "HideIcons";

        public Task ExecuteAsync()
        {
            SetIconVisibility(hide: true);
            ConsoleHelper.Success("Desktop icons hidden.");
            return Task.CompletedTask;
        }

        public Task RevertAsync()
        {
            SetIconVisibility(hide: false);
            ConsoleHelper.Success("Desktop icons restored.");
            return Task.CompletedTask;
        }

        private void SetIconVisibility(bool hide)
        {
            bool currentlyHidden = IsCurrentlyHidden();
            if (hide != currentlyHidden)
                Toggle();
        }

        private bool IsCurrentlyHidden()
        {
            using var key = Registry.CurrentUser.OpenSubKey(RegistryPath);
            if (key == null) return false;
            return (int)(key.GetValue(RegistryValue, 0) ?? 0) == 1;
        }

        private void Toggle()
        {
            IntPtr shellView = FindWindowEx(FindWindow("Progman", null), IntPtr.Zero, "SHELLDLL_DefView", null);

            if (shellView == IntPtr.Zero)
            {
                IntPtr workerW = IntPtr.Zero;
                do
                {
                    workerW = FindWindowEx(IntPtr.Zero, workerW, "WorkerW", null);
                    shellView = FindWindowEx(workerW, IntPtr.Zero, "SHELLDLL_DefView", null);
                } while (shellView == IntPtr.Zero && workerW != IntPtr.Zero);
            }

            if (shellView != IntPtr.Zero)
                SendMessage(shellView, 0x0111, (IntPtr)0x7402, IntPtr.Zero);
            else
                ConsoleHelper.Error("Could not find shell view window to toggle desktop icons.");
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string? lpszWindow);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
}
