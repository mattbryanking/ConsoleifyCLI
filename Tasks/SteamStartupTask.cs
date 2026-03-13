using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;
using Microsoft.Win32;

namespace ConsoleifyCLI.Tasks
{
    public class SteamStartupTask : IInstallOption
    {
        public int Id => 1;
        public string Name => "Set Steam to launch on startup in Big Picture";
        public string Category => "Configuration";
        public bool IsSelected { get; set; } = false;
        public bool HasWarning => false;
        public bool IsUninstallSupported => true;

        public Task ExecuteAsync()
        {
            string? steamPath = null;

            //try registry first
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    if (key != null)
                    {
                        string? registryPath = key.GetValue("SteamExe")?.ToString();
                        if (!string.IsNullOrWhiteSpace(registryPath))
                        {
                            steamPath = registryPath.Replace("/", "\\");
                        }
                    }
                }
            }
            catch {
                // don't throw this yet, we have more to try!!
            }

            // try default path next
            if (string.IsNullOrWhiteSpace(steamPath) || !File.Exists(steamPath)) {
                steamPath = @"C:\Program Files (x86)\Steam\steam.exe";
            }

            // if still not found, prompt user
            if (string.IsNullOrWhiteSpace(steamPath) || !File.Exists(steamPath))
            {
                string input = ConsoleHelper.Prompt("Could not find Steam. Enter full path to steam.exe:");
                steamPath = input.Replace("\"", "");
            }

            // final validation
            if (string.IsNullOrWhiteSpace(steamPath) || !File.Exists(steamPath))
            {
                // bail!
                ConsoleHelper.Error("Invalid path. Skipping Steam configuration.");
                return Task.CompletedTask;
            }

            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (key != null)
                {
                    string runCommand = $"\"{steamPath}\" -silent -bigpicture";
                    key.SetValue("Steam", runCommand);
                    ConsoleHelper.Success("Steam successfully set to Big Picture on startup!");
                }
                else
                {
                    ConsoleHelper.Error("Failed to open registry key for writing. Steam startup configuration not applied.");
                }
            }

            return Task.CompletedTask;
        }

        public Task RevertAsync()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (key != null && key.GetValue("Steam") != null)
                    {
                        key.DeleteValue("Steam");
                        ConsoleHelper.Success("Steam removed from startup.");
                    }
                    else
                    {
                        ConsoleHelper.Warning("Steam startup is not currently configured. Nothing to do.");
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.Error($"Failed to remove registry key: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}