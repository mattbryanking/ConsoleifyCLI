using System.Diagnostics;
using Microsoft.Win32;
using ConsoleifyCLI.UI;

namespace ConsoleifyCLI.Utilities
{
    public static class BackgroundServiceInstaller
    {
        private static readonly string _installDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Consoleify");
        private const string RunRegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static async Task InstallAsync(string downloadUrl, string exeName, string registryName)
        {
            string fullPath = Path.Combine(_installDir, exeName);
            if (!Directory.Exists(_installDir)) Directory.CreateDirectory(_installDir);

            // download if we don't already have it
            if (!File.Exists(fullPath))
            {
                ConsoleHelper.Info($"Downloading {exeName}...");
                using var client = new HttpClient();
                var data = await client.GetByteArrayAsync(downloadUrl);
                await File.WriteAllBytesAsync(fullPath, data);
            }
            else
            {
                ConsoleHelper.Info($"Found existing {exeName}. Skipping download.");
            }

            // add to startup
            ConsoleHelper.Info("Registering service for automatic startup...");
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunRegistryPath, true))
            {
                key?.SetValue(registryName, $"\"{fullPath}\"");
            }

            // punch it!
            ConsoleHelper.Info("Starting background service...");
            Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
        }

        public static void Uninstall(string exeName, string registryName)
        {
            string fullPath = Path.Combine(_installDir, exeName);

            // die!
            ConsoleHelper.Info($"Stopping {exeName}...");
            string processName = Path.GetFileNameWithoutExtension(exeName);
            var processes = Process.GetProcessesByName(processName);
            foreach (var p in processes) p.Kill();

            // remove from startup
            ConsoleHelper.Info("Removing from startup registry...");
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunRegistryPath, true))
            {
                key?.DeleteValue(registryName, false);
            }

            // die!!!!!!
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}