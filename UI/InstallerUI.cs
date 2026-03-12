using ConsolifyCLI.Core;

namespace ConsolifyCLI.UI
{
    public class InstallerUI
    {
        private const string Logo = @"
 ▄████████  ▄██████▄  ███▄▄▄▄      ▄████████  ▄██████▄   ▄█        ▄█     ▄████████ ▄██   ▄   
███    ███ ███    ███ ███▀▀▀██▄   ███    ███ ███    ███ ███       ███    ███    ███ ███   ██▄ 
███    █▀  ███    ███ ███   ███   ███    █▀  ███    ███ ███       ███▌   ███    █▀  ███▄▄▄███ 
███        ███    ███ ███   ███   ███        ███    ███ ███       ███▌  ▄███▄▄▄     ▀▀▀▀▀▀███ 
███        ███    ███ ███   ███ ▀███████████ ███    ███ ███       ███▌ ▀▀███▀▀▀     ▄██   ███ 
███    █▄  ███    ███ ███   ███          ███ ███    ███ ███       ███    ███        ███   ███ 
███    ███ ███    ███ ███   ███    ▄█    ███ ███    ███ ███▌    ▄ ███    ███        ███   ███ 
████████▀   ▀██████▀   ▀█   █▀   ▄████████▀   ▀██████▀  █████▄▄██ █▀     ███         ▀█████▀  ";

        private const string Description = @"
=============================================================================================
                     Tools to make your PC a more console-like experience.                   
=============================================================================================";

        private readonly List<IInstallOption> _options;

        public InstallerUI(List<IInstallOption> options)
        {
            _options = options;
        }

        public void ShowMenu()
        {
            bool isConfirming = false;

            while (!isConfirming)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(Logo);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(Description);
                Console.WriteLine();
                Console.ResetColor();

                Console.WriteLine("Press a number to toggle an option.");
                Console.WriteLine("Press ENTER to apply selected changes.");
                Console.WriteLine("Press ESC to exit.\n");

                foreach (var option in _options)
                {
                    string checkmark = option.IsSelected ? "[X]" : "[ ]";
                    Console.WriteLine($"{option.Id}. {checkmark} {option.Name}");
                }

                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
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