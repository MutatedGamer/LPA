using System.Collections.Concurrent;

namespace LPA.Application.EnabledTables
{
    internal class EnabledTablesManager
        : IEnabledTablesManager
    {
        private readonly ConcurrentDictionary<Guid, bool> tableEnablement = new ConcurrentDictionary<Guid, bool>();

        public Task<bool> IsEnabledAsync(Guid tableGuid)
        {
            AddGuidIfMissing(tableGuid);

            return Task.FromResult(this.tableEnablement[tableGuid]);
        }

        public Task ToggleEnabledAsync(Guid tableGuid)
        {
            AddGuidIfMissing(tableGuid);

            this.tableEnablement[tableGuid] = !this.tableEnablement[tableGuid];

            return Task.CompletedTask;
        }

        public Task DisableAll()
        {
            foreach (var table in this.tableEnablement.Keys)
            {
                this.tableEnablement[table] = false;
            }

            return Task.CompletedTask;
        }

        private void AddGuidIfMissing(Guid tableGuid)
        {
            if (!this.tableEnablement.ContainsKey(tableGuid))
            {
                this.tableEnablement.TryAdd(tableGuid, true);
            }
        }
    }
}