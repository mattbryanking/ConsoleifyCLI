using ConsolifyCLI.ConfigTasks;
using ConsolifyCLI.Core;
using ConsolifyCLI.UI;

namespace ConsolifyCLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var availableOptions = new List<IInstallOption>
            {
                new DummyTask(),
                new SteamStartupTask(),
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
            }

            ConsoleHelper.WriteLine("\nAll selected tasks finished. Press any key to exit.");
            Console.ReadKey();
        }
    }
}