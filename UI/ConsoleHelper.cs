namespace ConsoleifyCLI.UI
{
    public static class ConsoleHelper
    {
        public static void Clear()
        {
            Console.Clear();
        }
        public static void WriteLine()
        {
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void WriteLine(string message)
        {
            Console.ResetColor();
            Console.WriteLine(message);
        }

        public static void WriteLine(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }

        public static void Info(string message)
        {
            Console.ResetColor();
            Console.WriteLine($"    [~] {message}");
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"    [+] {message}");
            Console.ResetColor();
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"    [!] WARNING: {message}");
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"    [X] ERROR: {message}");
            Console.ResetColor();
        }
        
        public static string Prompt(string question)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"    > {question} ");
            Console.ResetColor();
            return Console.ReadLine() ?? string.Empty;
        }
    }
}