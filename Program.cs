using ConsolifyCLI.Core;
using ConsolifyCLI.UI;
using ConsolifyCLI.ConfigTasks;

namespace ConsolifyCLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var availableOptions = new List<IInstallOption>
            {
                new DummyTask(),
            };

            var renderer = new InstallerUI(availableOptions);
            renderer.ShowMenu();

            Console.Clear();
            Console.WriteLine("Starting Consolify Process...\n");

            var selectedOptions = availableOptions.Where(o => o.IsSelected).ToList();

            if (!selectedOptions.Any())
            {
                Console.WriteLine("No options were selected. Exiting...");
                return;
            }

            foreach (var option in selectedOptions)
            {
                try
                {
                    await option.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Failed to execute '{option.Name}': {ex.Message}");
                }
            }

            Console.WriteLine("\nAll selected tasks finished. Press any key to exit.");
            Console.ReadKey();
        }
    }
}