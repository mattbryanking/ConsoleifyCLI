using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;

namespace ConsoleifyCLI.ConfigTasks
{
    // just for testing purposes, will nuke this class later :)
    public class DummyTask : IInstallOption
    {
        public int Id => 0;
        public string Name => "Test Option!";
        public string Category => "Configuration";
        public bool IsSelected { get; set; } = false; 
        public bool HasWarning => false;

        public async Task ExecuteAsync()
        {
            ConsoleHelper.Info($"{Name}...");
            await Task.Delay(1000);
            ConsoleHelper.Success($"{Name} completed.");
        }

        public async Task RevertAsync()
        {
            ConsoleHelper.Info($"{Name}...");
            await Task.Delay(1000);
            ConsoleHelper.Success($"{Name} reverted.");
        }
    }
}