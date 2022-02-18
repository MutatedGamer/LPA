using LPA.Application.AvailableTables;
using LPA.Application.PluginLoading;

namespace LPA.Application.SDK
{
    internal interface ISdk
    {
        IEnumerable<IAvailableTable> AvailableTables { get; }

        IEnumerable<string> LoadedPlugins { get; }

        event EventHandler<PluginLoadedEventArgs> PluginLoaded;

        Task<bool> LoadPlugin(string directory);

        Task<bool> ProcessFile(string file, IEnumerable<IAvailableTable> enabledAvailableTables);
    }
}
