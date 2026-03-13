using ConsoleifyCLI.Core;

namespace ConsoleifyCLI.UI
{
    public class InstallerUI
    {
        private const string Logo = 
            @"▄████████  ▄██████▄  ███▄▄▄▄      ▄████████  ▄██████▄   ▄█        ▄████████  ▄█     ▄████████ ▄██   ▄   
███    ███ ███    ███ ███▀▀▀██▄   ███    ███ ███    ███ ███       ███    ███ ███    ███    ███ ███   ██▄ 
███    █▀  ███    ███ ███   ███   ███    █▀  ███    ███ ███       ███    █▀  ███▌   ███    █▀  ███▄▄▄███ 
███        ███    ███ ███   ███   ███        ███    ███ ███      ▄███▄▄▄     ███▌  ▄███▄▄▄     ▀▀▀▀▀▀███ 
███        ███    ███ ███   ███ ▀███████████ ███    ███ ███     ▀▀███▀▀▀     ███▌ ▀▀███▀▀▀     ▄██   ███ 
███    █▄  ███    ███ ███   ███          ███ ███    ███ ███       ███    █▄  ███    ███        ███   ███ 
███    ███ ███    ███ ███   ███    ▄█    ███ ███    ███ ███▌    ▄ ███    ███ ███    ███        ███   ███ 
████████▀   ▀██████▀   ▀█   █▀   ▄████████▀   ▀██████▀  █████▄▄██ ██████████ █▀     ███         ▀█████▀  
                                                        ▀                                                  ";

        private const string Description = 
@"========================================================================================================
                          Tools to make your PC a more console-like experience.                    
========================================================================================================";

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
                ConsoleHelper.WriteLine(modeText.PadLeft(53 + (modeText.Length / 2)), ModeColor);
                ConsoleHelper.WriteLine();

                ConsoleHelper.WriteLine("Controls: [1-9] Toggle | [ENTER] Apply | [ESC] Exit | [I] Install Mode | [U] Uninstall Mode");

                for (int i = 0; i < _options.Count; i++)
                {
                    var option = _options[i];
                    int displayId = i + 1;

                    if (i == 0 || option.Category != _options[i - 1].Category)
                    {
                        string header = $"{option.Category.ToUpper()}:";
                        ConsoleHelper.WriteLine();
                        ConsoleHelper.WriteLine($"{option.Category.ToUpper()}:", ConsoleColor.Yellow);
                        ConsoleHelper.WriteLine();
                    }

                    string checkmark = option.IsSelected ? "[X]" : "[ ]";

                    if (IsUninstallMode && !option.IsUninstallSupported)
                    {
                        option.IsSelected = false;
                        checkmark = "[ ]";
                        ConsoleHelper.WriteLine($"{displayId}. {checkmark} {option.Name}", ConsoleColor.DarkGray);
                    }
                    else if (option.IsSelected)
                    {
                        if (option.HasWarning)
                        {
                            ConsoleHelper.Write($"{displayId}. {checkmark} {option.Name}", ModeColor);
                            ConsoleHelper.WriteLine(" [USE AT OWN RISK!]", ConsoleColor.DarkYellow);
                        }
                        else
                        {
                            ConsoleHelper.WriteLine($"{displayId}. {checkmark} {option.Name}", ModeColor);
                        }
                    }
                    else
                    {
                        if (option.HasWarning)
                        {
                            ConsoleHelper.Write($"{displayId}. {checkmark} {option.Name}");
                            ConsoleHelper.WriteLine(" [USE AT OWN RISK!]", ConsoleColor.DarkYellow);
                        }
                        else
                        {
                            ConsoleHelper.WriteLine($"{displayId}. {checkmark} {option.Name}");
                        }
                    }
                }

                ConsoleHelper.WriteLine();
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
                    var selectedOption = pressedNumber > 0 && pressedNumber <= _options.Count ? _options[pressedNumber - 1] : null;

                    if (selectedOption != null && (!IsUninstallMode || selectedOption.IsUninstallSupported))
                    {
                        selectedOption.IsSelected = !selectedOption.IsSelected;
                    }
                }
            }
        }
    }
}