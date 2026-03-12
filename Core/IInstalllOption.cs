namespace ConsolifyCLI.Core
{
    public interface IInstallOption
    {
        int Id { get; }
        string Name { get; }
        bool IsSelected { get; set; }

        Task ExecuteAsync();
    }
}
