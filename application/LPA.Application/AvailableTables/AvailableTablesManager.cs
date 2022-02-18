using LPA.Application.PluginLoading;
using LPA.Application.SDK;
using LPA.UI.Tags;

namespace LPA.Application.AvailableTables
{
    internal class AvailableTablesManager
        : IAvailableTablesManager
    {
        private readonly ISdk sdk;

        private readonly List<IAvailableTable> tables = new List<IAvailableTable>();

        public AvailableTablesManager(ISdk sdk)
        {
            this.sdk = sdk;

            this.sdk.PluginLoaded += OnPluginLoaded;

            lock (this.tables)
            {
                this.tables.AddRange(this.sdk.AvailableTables);
            }
        }

        private void OnPluginLoaded(object? sender, PluginLoadedEventArgs e)
        {
            lock (this.tables)
            {
                this.tables.AddRange(e.Tables);
            }

            TagInvalidator.InvalidateTag(AvailableTablesTag.Instance);
        }

        public Task<IEnumerable<IAvailableTable>> GetAvailableTablesAsync()
        {
            lock (this.tables)
            {
                return Task.FromResult(this.tables.ToArray().AsEnumerable());
            }
        }
    }
}
