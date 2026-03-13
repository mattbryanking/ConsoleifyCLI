namespace ConsolifyCLI.Core
{
    public interface IInstallOption
    {
        int Id { get; }
        string Name { get; }
        string Category { get; }
        bool IsSelected { get; set; }

        Task ExecuteAsync();
        Task RevertAsync();
    }
}