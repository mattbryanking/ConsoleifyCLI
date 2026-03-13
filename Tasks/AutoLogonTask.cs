using System.Diagnostics;
using System.IO.Compression;
using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;

namespace ConsoleifyCLI.Tasks
{
    public class AutoLogonTask : IInstallOption
    {
        public string Name => "Auto log-on to Windows (Sysinternals)";
        public string Category => "Utilities";

        public bool IsSelected { get; set; } = false;
        public bool HasWarning => false;
        public bool IsUninstallSupported => true;

        private readonly string _zipUrl = "https://download.sysinternals.com/files/AutoLogon.zip";
        private readonly string _workDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Consoleify");

        public async Task ExecuteAsync()
        {
            string exePath = await PrepareAutoLogonExeAsync();

            ConsoleHelper.Info("Opening AutoLogon tool. Please enter your password and click 'Enable'.");

            RunAndPause(exePath);
            CleanupFiles();

            ConsoleHelper.Success("AutoLogon configuration complete. Temp files cleaned up.");
        }

        public async Task RevertAsync()
        {
            string exePath = await PrepareAutoLogonExeAsync();

            ConsoleHelper.Info("Opening AutoLogon tool. Please click 'Disable'.");

            RunAndPause(exePath);
            CleanupFiles();

            ConsoleHelper.Success("AutoLogon removal complete. Temp files cleaned up.");
        }

        private async Task<string> PrepareAutoLogonExeAsync()
        {
            if (!Directory.Exists(_workDir)) Directory.CreateDirectory(_workDir);

            string zipPath = Path.Combine(_workDir, "autologon.zip");
            string exePath = Path.Combine(_workDir, "Autologon64.exe");

            if (File.Exists(exePath))
            {
                return exePath;
            }

            ConsoleHelper.Info("Downloading Sysinternals tool from Microsoft...");
            using var client = new HttpClient();
            var data = await client.GetByteArrayAsync(_zipUrl);
            await File.WriteAllBytesAsync(zipPath, data);

            ConsoleHelper.Info("Extracting tool...");
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                // i hope you're not gaming on 32 bit!
                var entry = archive.GetEntry("Autologon64.exe");
                entry?.ExtractToFile(exePath, true);
            }

            return exePath;
        }

        private void RunAndPause(string exePath)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = true
            };

            using var process = Process.Start(startInfo);

            if (process != null)
            {
                process.WaitForExit();
            }
        }

        private void CleanupFiles()
        {
            ConsoleHelper.Info("Cleaning up temporary files...");

            string zipPath = Path.Combine(_workDir, "autologon.zip");
            string exePath = Path.Combine(_workDir, "Autologon64.exe");

            if (File.Exists(exePath)) File.Delete(exePath);
            if (File.Exists(zipPath)) File.Delete(zipPath);

            // if the appdata folder is empty now, remove it too
            if (Directory.Exists(_workDir) && !Directory.EnumerateFileSystemEntries(_workDir).Any())
            {
                Directory.Delete(_workDir);
            }
        }
    }
}