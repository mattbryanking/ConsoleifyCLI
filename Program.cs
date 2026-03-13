using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;
using ConsoleifyCLI.ConfigTasks;
using System.Diagnostics;
using System.Security.Principal;

namespace ConsoleifyCLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var availableOptions = new List<IInstallOption>
            {
                new DummyTask(),
                new SteamStartupTask(),
                new AutoHideTaskbarTask(),
                new BlackDesktopTask(),
                new PowerSleepTask(),
            };

            var installer = new InstallerUI(availableOptions);
            installer.ShowMenu();

            ConsoleHelper.Clear();
            ConsoleHelper.WriteLine("Starting Consolify Process...\n");

            var selectedOptions = availableOptions.Where(o => o.IsSelected).ToList();

            if (!selectedOptions.Any())
            {
                ConsoleHelper.WriteLine("No options were selected. Exiting...");
                return;
            }

            foreach (var option in selectedOptions)
            {
                try
                {
                    if (installer.IsRevertMode)
                    {
                        ConsoleHelper.Info($"Reverting: {option.Name}...");
                        await option.RevertAsync();
                    }
                    else
                    {
                        ConsoleHelper.Info($"Configuring: {option.Name}...");
                        await option.ExecuteAsync();
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.Error($"Failed to execute '{option.Name}': {ex.Message}");
                }

                ConsoleHelper.WriteLine();
            }

            ConsoleHelper.WriteLine("All selected tasks finished. Press any key to exit.");
            Console.ReadKey();
        }
    }
}