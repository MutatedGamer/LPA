namespace LPA.Application.PluginLoading
{
    public interface IPluginLoader
    {
        IEnumerable<string> LoadedPlugins { get; }

        Task LoadPluginAsync();

        Task LoadPluginAsync(string path);
    }
}
