using LPA.Application.AvailableTables;
using LPA.Application.SDK;

namespace LPA.Application.EnabledTables
{
    internal class EnabledTablesManager
        : IEnabledTablesManager
    {
        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        private readonly Dictionary<Guid, bool> tableEnablement = new Dictionary<Guid, bool>();

        public EnabledTablesManager(IAvailableTablesManager availableTablesManager, ISdk sdk)
        {
            sdk.PluginLoaded += Sdk_PluginLoaded;

            this.rwLock.EnterWriteLock();
            foreach (var availableTable in availableTablesManager.GetAvailableTablesAsync().Result)
            {
                this.tableEnablement.TryAdd(availableTable.Guid, true);
            }
            this.rwLock.ExitWriteLock();
        }

        private void Sdk_PluginLoaded(object? sender, PluginLoading.PluginLoadedEventArgs e)
        {
            this.rwLock.EnterWriteLock();
            foreach (var availableTable in e.Tables)
            {
                this.tableEnablement.TryAdd(availableTable.Guid, true);
            }
            this.rwLock.ExitWriteLock();
        }

        public Task<bool> IsEnabledAsync(Guid tableGuid)
        {
            var value = GetOrAddTableGuid(tableGuid);

            return Task.FromResult(value);
        }

        public Task ToggleEnabledAsync(Guid tableGuid)
        {
            var value = GetOrAddTableGuid(tableGuid);

            this.rwLock.EnterWriteLock();
            this.tableEnablement[tableGuid] = !value;
            this.rwLock.ExitWriteLock();

            return Task.CompletedTask;
        }

        public Task DisableAll()
        {
            this.rwLock.EnterWriteLock();
            foreach (var table in this.tableEnablement.Keys)
            {
                this.tableEnablement[table] = false;
            }
            this.rwLock.ExitWriteLock();

            return Task.CompletedTask;
        }

        private bool GetOrAddTableGuid(Guid tableGuid)
        {
            this.rwLock.EnterReadLock();
            var exists = this.tableEnablement.TryGetValue(tableGuid, out var result);
            this.rwLock.ExitReadLock();

            if (exists)
            {
                return result;
            }
            else
            {
                bool addResult;

                this.rwLock.EnterWriteLock();
                if (this.tableEnablement.ContainsKey(tableGuid))
                {
                    addResult = this.tableEnablement[tableGuid];
                }
                else
                {
                    this.tableEnablement.Add(tableGuid, true);
                    addResult = true;
                }
                this.rwLock.ExitWriteLock();

                return addResult;
            }
        }
    }
}