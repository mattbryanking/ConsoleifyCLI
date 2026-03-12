using ConsolifyCLI.Core;

namespace ConsolifyCLI.ConfigTasks
{
    // just for testing purposes, will nuke this class later :)
    public class DummyTask : IInstallOption
    {
        public int Id => 1;
        public string Name => "Test Option!";
        public bool IsSelected { get; set; } = false; 

        public async Task ExecuteAsync()
        {
            Console.WriteLine($"Executing: {Name}...");
            await Task.Delay(1000);
            Console.WriteLine($"[Success] {Name} completed.");
        }
    }
}