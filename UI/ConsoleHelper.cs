namespace ConsoleifyCLI.UI
{
    public static class ConsoleHelper
    {
        public static void Clear()
        {
            Console.ResetColor();
            Console.Clear();
        }
        
        public static void Write(string message = "", ConsoleColor color = ConsoleColor.Black)
        {
            if (color != ConsoleColor.Black)
            {
                Console.ForegroundColor = color;
            }
            Console.Write(message);
            Console.ResetColor();
        }

        public static void WriteLine(string message = "", ConsoleColor color = ConsoleColor.Black)
        {
            if (color != ConsoleColor.Black)
            {
                Console.ForegroundColor = color;
            }
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Info(string message)
        {
            Console.WriteLine($"    [~] {message}");
            Console.ResetColor();
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