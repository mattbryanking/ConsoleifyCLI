using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI;

namespace ConsoleifyCLI.Utilities
{
    public class NetworkCECUtility : IInstallOption
    {
        public string Name => "Network CEC (turns on TV on keyboard/gamepad input)";
        public string Category => "Utilities";

        public bool IsSelected { get; set; } = false;
        public bool HasWarning => false;
        public bool IsInstallSupported => true;
        public bool IsUninstallSupported => true;

        private readonly string _downloadUrl = "https://github.com/mattbryanking/Consoleify.GoogleTVNetworkCEC/releases/latest/download/Consoleify.GoogleTVNetworkCEC.exe";
        private readonly string _exeName = "Consoleify.GoogleTVNetworkCEC.exe";
        private readonly string _registryName = "ConsoleifyNetworkCEC";

        public async Task ExecuteAsync()
        {
            await BackgroundServiceInstaller.InstallAsync(_downloadUrl, _exeName, _registryName, true);
            ConsoleHelper.Success("Network CEC installed and running in background!");
        }

        public Task RevertAsync()
        {
            BackgroundServiceInstaller.Uninstall(_exeName, _registryName);
            ConsoleHelper.Success("Network CEC removed successfully.");
            return Task.CompletedTask;
        }
    }
}