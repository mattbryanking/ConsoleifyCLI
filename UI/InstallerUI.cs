using ConsoleifyCLI.Core;

namespace ConsoleifyCLI.UI
{
    public class InstallerUI
    {
        private const string Logo = @"
 ▄████████  ▄██████▄  ███▄▄▄▄      ▄████████  ▄██████▄   ▄█          ▄████████  ▄█     ▄████████ ▄██   ▄   
███    ███ ███    ███ ███▀▀▀██▄   ███    ███ ███    ███ ███         ███    ███ ███    ███    ███ ███   ██▄ 
███    █▀  ███    ███ ███   ███   ███    █▀  ███    ███ ███         ███    █▀  ███▌   ███    █▀  ███▄▄▄███ 
███        ███    ███ ███   ███   ███        ███    ███ ███        ▄███▄▄▄     ███▌  ▄███▄▄▄     ▀▀▀▀▀▀███ 
███        ███    ███ ███   ███ ▀███████████ ███    ███ ███       ▀▀███▀▀▀     ███▌ ▀▀███▀▀▀     ▄██   ███ 
███    █▄  ███    ███ ███   ███          ███ ███    ███ ███         ███    █▄  ███    ███        ███   ███ 
███    ███ ███    ███ ███   ███    ▄█    ███ ███    ███ ███▌    ▄   ███    ███ ███    ███        ███   ███ 
████████▀   ▀██████▀   ▀█   █▀   ▄████████▀   ▀██████▀  █████▄▄██   ██████████ █▀     ███         ▀█████▀  
                                                        ▀                                                  ";

        private const string Description = 
@"==========================================================================================================
                           Tools to make your PC a more console-like experience.                    
==========================================================================================================";

        private readonly List<IInstallOption> _options;

        public bool IsUninstallMode { get; private set; } = false;

        public ConsoleColor ModeColor => IsUninstallMode ? ConsoleColor.Red : ConsoleColor.Green;

        public InstallerUI(List<IInstallOption> options)
        {
            _options = options;
        }

        public void ShowMenu()
        {
            bool isConfirming = false;

            while (!isConfirming)
            {
                ConsoleHelper.Clear();
                ConsoleHelper.WriteLine(Logo, ConsoleColor.DarkCyan);
                ConsoleHelper.WriteLine(Description, ModeColor);

                ConsoleHelper.WriteLine();
                string modeText = IsUninstallMode ? "[ CURRENT MODE: UNINSTALL ]" : "[ CURRENT MODE: INSTALL ]";
                ConsoleHelper.WriteLine(modeText.PadLeft(54 + (modeText.Length / 2)), ModeColor);
                ConsoleHelper.WriteLine();

                ConsoleHelper.WriteLine("Controls:");
                ConsoleHelper.WriteLine("1-9:   Toggle option on/off");
                ConsoleHelper.WriteLine("ENTER: Apply selected changes");
                ConsoleHelper.WriteLine("ESC:   Exit without applying");
                ConsoleHelper.WriteLine("I:     Switch to INSTALL mode");
                ConsoleHelper.WriteLine("U:     Switch to UNINSTALL mode");
                ConsoleHelper.WriteLine();

                foreach (var option in _options)
                {
                    string checkmark = option.IsSelected ? "[X]" : "[ ]";

                    if (option.IsSelected)
                    {
                        if (option.HasWarning)
                        {
                            ConsoleHelper.Write($"{option.Id}. {checkmark} {option.Name}", ModeColor);
                            ConsoleHelper.WriteLine(" [USE AT OWN RISK!]", ConsoleColor.Yellow);
                        }
                        else
                        {
                            ConsoleHelper.WriteLine($"{option.Id}. {checkmark} {option.Name}", ModeColor);
                        }
                    }
                    else
                    {
                        if (option.HasWarning)
                        {
                            ConsoleHelper.Write($"{option.Id}. {checkmark} {option.Name}");
                            ConsoleHelper.WriteLine(" [USE AT OWN RISK!]", ConsoleColor.Yellow);
                        }
                        else
                        {
                            ConsoleHelper.WriteLine($"{option.Id}. {checkmark} {option.Name}");
                        }
                    }
                }

                var keyInfo = Console.ReadKey(true);
                char upperKey = char.ToUpper(keyInfo.KeyChar);

                if (upperKey == 'I')
                {
                    IsUninstallMode = false;
                }
                else if (upperKey == 'U')
                {
                    IsUninstallMode = true;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    ConsoleHelper.Clear();
                    Environment.Exit(0);
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    isConfirming = true;
                }
                else if (char.IsDigit(keyInfo.KeyChar))
                {
                    int pressedNumber = int.Parse(keyInfo.KeyChar.ToString());
                    var selectedOption = _options.FirstOrDefault(o => o.Id == pressedNumber);

                    if (selectedOption != null)
                    {
                        selectedOption.IsSelected = !selectedOption.IsSelected;
                    }
                }
            }
        }
    }
}