using ConsoleifyCLI.Utilities;

namespace ConsoleifyCLI.UI
{
    public static class ConsoleHelper
    {
        public static void Clear()
        {
            Console.ResetColor();
            Console.Clear();
        }
        
        public static void Write(string message = "", ConsoleColor? color = null)
        {
            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
            }
            Console.Write(message);
            Console.ResetColor();
        }

        public static void WriteLine(string message = "", ConsoleColor? color = null)
        {
            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
            }
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Info(string message)
        {
            Console.WriteLine($"    [~] {message}");
            Console.ResetColor();
            Logger.Info(message);
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"    [+] {message}");
            Console.ResetColor();
            Logger.Success(message);
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"    [!] WARNING: {message}");
            Console.ResetColor();
            Logger.Warning(message);
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"    [X] ERROR: {message}");
            Console.ResetColor();
            Logger.Error(message);
        }

        public static string Prompt(string question)
        {
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"    > {question} ");
            Console.ResetColor();
            Console.CursorVisible = false;
            return Console.ReadLine() ?? string.Empty;
        }
    }
}