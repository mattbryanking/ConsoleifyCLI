using ConsoleifyCLI.Core;
using ConsoleifyCLI.Tasks;
using ConsoleifyCLI.UI;
using ConsoleifyCLI.Utilities;

namespace ConsoleifyCLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Consoleify PC Installer";
            Console.CursorVisible = false;
            var availableOptions = new List<IInstallOption>
            {
                new SteamStartupTask(),
                new AutoHideTaskbarTask(),
                new BlackDesktopTask(),
                new PowerSleepTask(),
                new AutoLogonTask(),
                new DesktopIconUtility(),
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
                    if (installer.IsUninstallMode)
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