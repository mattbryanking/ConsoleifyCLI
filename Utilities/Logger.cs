namespace ConsoleifyCLI.Utilities
{
    public static class Logger
    {
        private static readonly string LogDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Consoleify");
        private static readonly string LogPath = Path.Combine(LogDirectory, "consoleify.log");
        private static readonly object _lock = new();

        public static void Log(string level, string message)
        {
            try
            {
                Directory.CreateDirectory(LogDirectory);
                string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level,-7}] [CLI] {message}";
                lock (_lock)
                {
                    File.AppendAllText(LogPath, entry + Environment.NewLine);
                }
            }
            catch { /* logging must never crash the app */ }
        }

        public static void Info(string message) => Log("INFO", message);
        public static void Success(string message) => Log("SUCCESS", message);
        public static void Warning(string message) => Log("WARN", message);
        public static void Error(string message) => Log("ERROR", message);
    }
}
