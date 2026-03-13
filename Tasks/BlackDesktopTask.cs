using System.Runtime.InteropServices;
using ConsoleifyCLI.Core;
using ConsoleifyCLI.UI; 
using Microsoft.Win32;

namespace ConsoleifyCLI.Tasks
{
    public class BlackDesktopTask : IInstallOption
    {
        public int Id => 3;
        public string Name => "Set desktop background to black";
        public string Category => "Configuration";
        public bool IsSelected { get; set; } = false;
        public bool HasWarning => false;
        public bool IsUninstallSupported => false;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        [DllImport("user32.dll")]
        private static extern bool SetSysColors(int cElements, int[] lpaElements, uint[] lpaRgbValues);

        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        private const int COLOR_DESKTOP = 1;

        public Task ExecuteAsync()
        {
            // registry edits to make the changes permanent 
            Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Wallpaper", "");
            Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Colors", "Background", "0 0 0");

            // live changes so this happens right now! (don't need to wait for reboot)
            // nuke current wallpaper
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, "", SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            // set wallpaper to black color
            SetSysColors(1, new int[] { COLOR_DESKTOP }, new uint[] { 0x000000 });

            ConsoleHelper.Success("Desktop background set to solid black.");

            return Task.CompletedTask;
        }

        public Task RevertAsync()
        {
            return Task.CompletedTask;
        }
    }
}