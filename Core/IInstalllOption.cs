namespace ConsoleifyCLI.Core
{
    public interface IInstallOption
    {
        int Id { get; }
        string Name { get; }
        string Category { get; }
        bool IsSelected { get; set; }
        bool HasWarning { get; }
        bool IsUninstallSupported { get; }

        Task ExecuteAsync();
        Task RevertAsync();
    }
}