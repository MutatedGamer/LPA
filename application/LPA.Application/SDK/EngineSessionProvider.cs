using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using LPA.Application.Sessions.Provider;
using Microsoft.Performance.SDK.Processing;
using Microsoft.Performance.Toolkit.Engine;

namespace LPA.Application.SDK
{
    internal class EngineSessionProvider
        : ISessionProvider
    {
        private readonly RuntimeExecutionResults result;
        private readonly ReadOnlyDictionary<Guid, TableDescriptor> tables;

        private readonly ConcurrentDictionary<Guid, ITableResult> builtTables = new();

        public EngineSessionProvider(IEnumerable<TableDescriptor> enabledTables, RuntimeExecutionResults result)
        {
            this.result = result;
            this.tables = new ReadOnlyDictionary<Guid, TableDescriptor>(enabledTables.ToDictionary(table => table.Guid, table => table));
        }

        public async Task<(string name, Guid guid)[]> GetDefaultConfigColumnsAsync(Guid tableId)
        {
            await BuildIfNecessary(tableId);

            var config = GetConfigToUse(tableId);
            return config.Columns.Select(column => (column.Metadata.Name, column.Metadata.Guid)).ToArray();
        }

        public async Task<string[][]> GetTableDataAsync(Guid tableId)
        {
            await BuildIfNecessary(tableId);

            if (this.result.IsTableDataAvailable(this.tables[tableId]) == false)
            {
                return new string[][] { new string[0] };
            }

            var table = this.builtTables[tableId];

            var rowCount = table.RowCount;
            if (rowCount == 0)
            {
                return new string[][] { new string[0] };
            }

            // TODO: pass columns
            var config = GetConfigToUse(tableId);

            var colGuids = config.Columns.Select(col => col.Metadata.Guid).ToHashSet();
            var columns = table.Columns.Where(col => colGuids.Contains(col.Configuration.Metadata.Guid)).ToArray();

            var data = new List<string[]>();

            for (int i = 0; i < rowCount; i++)
            {
                var row = new List<string>();

                foreach (var column in columns)
                {
                    row.Add(column.Project(i)?.ToString());
                }

                data.Add(row.ToArray());
            }

            return data.ToArray();
        }

        public Task<Guid[]> GetSessionTables()
        {
            return Task.FromResult(this.tables.Keys.ToArray());
        }

        public async Task IsTableDataAvailable(Guid tableId)
        {
            await BuildIfNecessary(tableId);
        }

        private TableConfiguration GetConfigToUse(Guid tableId)
        {
            if (this.builtTables[tableId].DefaultConfiguration != null)
            {
                return this.builtTables[tableId].DefaultConfiguration;
            }
            else if (this.builtTables[tableId].BuiltInTableConfigurations.Any())
            {
                return this.builtTables[tableId].BuiltInTableConfigurations.First();
            }
            else
            {
                var builtIn = this.tables[tableId].PrebuiltTableConfigurations;
                var configs = builtIn.Configurations;
                if (!string.IsNullOrEmpty(builtIn.DefaultConfigurationName))
                {
                    return configs.First(config => config.Name == builtIn.DefaultConfigurationName);
                }
                else
                {
                    return configs.First();
                }
            }
        }

        private Task BuildIfNecessary(Guid tableId)
        {
            if (!this.builtTables.ContainsKey(tableId))
            {
                var result = this.result.BuildTable(this.tables[tableId]);

                this.builtTables.TryAdd(tableId, result);
            }

            return Task.CompletedTask;
        }
    }
}