using System.Collections.ObjectModel;
using LPA.Application.Sessions.Provider;
using LPA.Application.Sessions.Tables;
using LPA.Application.Sessions.Tables.TableConfigs;
using Microsoft.Performance.SDK.Processing;
using Microsoft.Performance.Toolkit.Engine;

namespace LPA.Application.SDK
{
    internal class EngineSessionProvider
        : ISessionProvider
    {
        private readonly RuntimeExecutionResults result;
        private readonly ReadOnlyDictionary<Guid, TableDescriptor> tables;

        private readonly ReadOnlyDictionary<Guid, Lazy<ITableResult?>> builtTables;
        private readonly ReadOnlyDictionary<Guid, Lazy<ReadOnlyDictionary<Guid, TableConfiguration>>> tableConfigurations;

        public EngineSessionProvider(IEnumerable<TableDescriptor> enabledTables, RuntimeExecutionResults result)
        {
            this.result = result;
            this.tables = new ReadOnlyDictionary<Guid, TableDescriptor>(enabledTables.ToDictionary(table => table.Guid, table => table));

            this.builtTables = new ReadOnlyDictionary<Guid, Lazy<ITableResult?>>(
                enabledTables.ToDictionary(
                    table => table.Guid,
                    table => new Lazy<ITableResult?>(() =>
                    {
                        try
                        {
                            return this.result.BuildTable(this.tables[table.Guid]);
                        }
                        catch
                        {
                            return null;
                        }
                    }, LazyThreadSafetyMode.ExecutionAndPublication)));

            this.tableConfigurations = new ReadOnlyDictionary<Guid, Lazy<ReadOnlyDictionary<Guid, TableConfiguration>>>(
                enabledTables.ToDictionary(
                    table => table.Guid,
                    table => new Lazy<ReadOnlyDictionary<Guid, TableConfiguration>>(() =>
                   {
                       var configs = new Dictionary<Guid, TableConfiguration>();

                       foreach (var config in table.PrebuiltTableConfigurations)
                       {
                           configs.Add(Guid.NewGuid(), config);
                       }

                       var builtTable = this.builtTables[table.Guid].Value;

                       if (builtTable != null)
                       {
                           if (builtTable.BuiltInTableConfigurations.Any())
                           {
                               foreach (var config in builtTable.BuiltInTableConfigurations)
                               {
                                   configs.Add(Guid.NewGuid(), config);
                               }
                           }
                           else
                           {
                               // TODO: create a config with all columns
                           }
                       }

                       return new ReadOnlyDictionary<Guid, TableConfiguration>(configs);
                   }, LazyThreadSafetyMode.ExecutionAndPublication)));
        }

        public Task<ITableConfiguration[]> GetConfigurationsAsync(Guid tableId)
        {
            var configs = this.tableConfigurations[tableId].Value;

            var sdkTableConfigs = configs.Select(kvp => (ITableConfiguration)new SdkTableConfiguration(kvp.Key, kvp.Value));
            return Task.FromResult(sdkTableConfigs.ToArray());
        }

        public Task<ISessionTableInfo> GetTableInfoAsync(Guid tableId)
        {
            var table = this.tables[tableId];
            return Task.FromResult<ISessionTableInfo>(new SdkTableInfo(table, string.Empty));
        }


        public Task<Guid[]> GetSessionTables()
        {
            return Task.FromResult(this.tables.Keys.ToArray());
        }

        public Task<ITableConfiguration?> GetConfigurationAsync(Guid tableId, Guid configId)
        {
            var configs = this.tableConfigurations[tableId].Value;
            if (configs.TryGetValue(configId, out var tableConfiguration))
            {
                var config = new SdkTableConfiguration(configId, tableConfiguration);
                return Task.FromResult<ITableConfiguration?>(config);
            }
            else
            {
                return Task.FromResult<ITableConfiguration?>(null);
            }
        }

        public Task<Guid> GetDefaultConfiguration(Guid tableId)
        {
            var table = this.tables[tableId];

            if (!string.IsNullOrEmpty(table.PrebuiltTableConfigurations.DefaultConfigurationName))
            {
                foreach (var kvp in this.tableConfigurations[tableId].Value)
                {
                    if (kvp.Value.Name == table.PrebuiltTableConfigurations.DefaultConfigurationName)
                    {
                        return Task.FromResult(kvp.Key);
                    }
                }
            }

            var builtTable = this.builtTables[tableId].Value;
            if (builtTable?.DefaultConfiguration != null)
            {
                foreach (var kvp in this.tableConfigurations[tableId].Value)
                {
                    if (kvp.Value.Name == builtTable.DefaultConfiguration.Name)
                    {
                        return Task.FromResult(kvp.Key);
                    }
                }
            }

            var configs = this.tableConfigurations[tableId].Value;
            if (configs.Any())
            {
                return Task.FromResult(configs.Keys.First());
            }
            else
            {
                return Task.FromResult(Guid.Empty);
            }
        }

        public Task<int> GetRowCountAsync(Guid tableId)
        {
            return Task.FromResult(this.builtTables[tableId].Value?.RowCount ?? 0);
        }

        public Task<string[][]> GetRowsAsync(Guid tableId, IEnumerable<Guid> columns, int start, int count)
        {
            var table = this.builtTables[tableId].Value;

            if (table == null)
            {
                return Task.FromResult(new string[1][] { new string[0] });
            }

            var columnsToUse = columns.Select(columnGuid => table.Columns.FirstOrDefault(col => col.Configuration.Metadata.Guid == columnGuid)).ToList();

            var rows = new List<string[]>(count);

            for (int i = start; i < start + count; i++)
            {
                if (i >= table.RowCount)
                {
                    break;
                }

                var row = new List<string>(columnsToUse.Count);
                foreach (var column in columnsToUse)
                {
                    row.Add(column?.Project(i)?.ToString() ?? string.Empty);
                }

                rows.Add(row.ToArray());
            }

            return Task.FromResult(rows.ToArray());
        }
    }
}