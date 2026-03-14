using System.Diagnostics;
using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;

namespace ConsoleifyCLI.Tasks
{
    public class ChrisWinUtilTask : IInstallOption
    {
        public string Name => "Run Chris Titus Tech's Windows Utility";
        public string Category => "Configuration";

        public bool IsSelected { get; set; } = false;
        public bool HasWarning => false;
        public bool IsUninstallSupported => true;

        public Task ExecuteAsync()
        {
            Run();

            ConsoleHelper.Success("Chris Titus Tech's Windows Utility ran successfully.");
            return Task.CompletedTask;
        }

        public Task RevertAsync()
        {
            Run();

            ConsoleHelper.Success("Chris Titus Tech's Windows Utility ran successfully.");
            return Task.CompletedTask;
        }

        public void Run()
        {
            string script = "irm https://christitus.com/win | iex";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
                UseShellExecute = true,
                Verb = "runas" 
            };

            using (Process? process = Process.Start(psi))
            {
                ConsoleHelper.Info("Utility launched successfully in a new window.");
                process?.WaitForExit();
            }
        }
    }
}