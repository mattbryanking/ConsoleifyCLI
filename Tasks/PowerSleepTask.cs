using System.Diagnostics;
using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;

namespace ConsoleifyCLI.Tasks
{
    public class PowerSleepTask : IInstallOption
    {
        public int Id => 4;
        public string Name => "Set physical PC power button to sleep";
        public string Category => "Configuration";

        public bool IsSelected { get; set; } = false;
        public bool HasWarning => false;
        public bool IsUninstallSupported => true;

        private const int SLEEP_ACTION = 1;
        private const int SHUTDOWN_ACTION = 3;

        public Task ExecuteAsync()
        {
            SetPowerButtonAction(SLEEP_ACTION);

            ConsoleHelper.Success("Power button successfully mapped to Sleep.");
            return Task.CompletedTask;
        }

        public Task RevertAsync()
        {
            SetPowerButtonAction(SHUTDOWN_ACTION);

            ConsoleHelper.Success("Power button restored to default Shut Down.");
            return Task.CompletedTask;
        }

        private void SetPowerButtonAction(int actionValue)
        {
            // ac and dc values in case we're using battery
            RunPowerCfg($"-setacvalueindex SCHEME_CURRENT SUB_BUTTONS PBUTTONACTION {actionValue}");
            RunPowerCfg($"-setdcvalueindex SCHEME_CURRENT SUB_BUTTONS PBUTTONACTION {actionValue}");

            // punch it!
            RunPowerCfg("-setactive SCHEME_CURRENT");
        }

        private void RunPowerCfg(string arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "powercfg",
                Arguments = arguments,
                CreateNoWindow = true, 
                UseShellExecute = false
            };

            using var process = Process.Start(startInfo);
            process?.WaitForExit();

            if (process != null && process.ExitCode != 0)
            {
                throw new Exception($"powercfg exited with code {process.ExitCode}");
            }
        }
    }
}