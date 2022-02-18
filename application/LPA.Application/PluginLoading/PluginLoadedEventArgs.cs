using LPA.Application.AvailableTables;

namespace LPA.Application.PluginLoading
{
    public class PluginLoadedEventArgs
        : EventArgs
    {
        public PluginLoadedEventArgs(string pluginName, IEnumerable<IAvailableTable> tables)
        {
            this.PluginName = pluginName;
            this.Tables = tables;
        }

        public string PluginName { get; }
        public IEnumerable<IAvailableTable> Tables { get; }
    }
}
