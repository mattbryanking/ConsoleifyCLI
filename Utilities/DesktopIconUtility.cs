using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;

namespace ConsoleifyCLI.Utilities
{
    public class DesktopIconUtility : IInstallOption
    {
        public string Name => "Desktop icon auto-hider";
        public string Category => "Utilities";

        public bool IsSelected { get; set; } = false;
        public bool HasWarning => false;
        public bool IsInstallSupported => true;
        public bool IsUninstallSupported => true;

        private readonly string _downloadUrl = "https://github.com/YourUser/YourRepo/raw/main/Consoleify.DesktopCleaner.exe";
        private readonly string _exeName = "Consoleify.DesktopCleaner.exe";
        private readonly string _registryName = "ConsoleifyDesktopCleaner";

        public async Task ExecuteAsync()
        {
            await BackgroundServiceInstaller.InstallAsync(_downloadUrl, _exeName, _registryName);
            ConsoleHelper.Success("Desktop Cleaner installed and running in background!");
        }

        public Task RevertAsync()
        {
            BackgroundServiceInstaller.Uninstall(_exeName, _registryName);
            ConsoleHelper.Success("Desktop Cleaner removed successfully.");
            return Task.CompletedTask;
        }
    }
}