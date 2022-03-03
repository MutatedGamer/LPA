using LPA.Application.AvailableTables;
using LPA.Application.PluginLoading;
using LPA.Application.Sessions;
using Microsoft.Performance.SDK.Processing;
using Microsoft.Performance.SDK.Runtime;
using Microsoft.Performance.Toolkit.Engine;

namespace LPA.Application.SDK
{
    internal class SdkWrapper
        : ISdk
    {
        private readonly List<string> loadedDirectories = new List<string>();
        private readonly List<IAvailableTable> availableTables = new List<IAvailableTable>();

        private PluginSet? pluginSet;

        private readonly ISessionsManager sessionsManager;

        public SdkWrapper(ISessionsManager sessionsManager)
        {
            this.sessionsManager = sessionsManager;
        }

        public IEnumerable<IAvailableTable> AvailableTables
        {
            get
            {
                lock (this.availableTables)
                {
                    return this.availableTables.ToList();
                }
            }
        }

        public IEnumerable<string> LoadedPlugins
        {
            get
            {
                string[] result;
                lock (this.loadedDirectories)
                {
                    result = new string[this.loadedDirectories.Count];
                    this.loadedDirectories.CopyTo(result);
                    return result.AsEnumerable();
                }
            }
        }

        public event EventHandler<PluginLoadedEventArgs>? PluginLoaded;

        public Task<bool> LoadPlugin(string directory)
        {
            IEnumerable<SdkTableInfo> newTables;

            return Task.Run(() =>
            {
                lock (this.loadedDirectories)
                {
                    try
                    {
                        var oldProcessingSources = new List<ProcessingSourceReference>();
                        if (this.pluginSet != null)
                        {
                            oldProcessingSources.AddRange(this.pluginSet.ProcessingSourceReferences);
                        }

                        this.pluginSet = PluginSet.Load(this.loadedDirectories.Concat(new string[] { directory }));

                        //
                        // We temporarily create an engine in order to populate available tables
                        //

                        var dataSourceSet = DataSourceSet.Create(this.pluginSet, false);
                        var info = new EngineCreateInfo(dataSourceSet.AsReadOnly());

                        var engine = Engine.Create(info);

                        var currentGuids = this.availableTables.Select(t => t.Guid).ToHashSet();
                        newTables = engine.AvailableTables.Where(td => !currentGuids.Contains(td.Guid)).Select(table => new SdkTableInfo(table, directory));

                        engine.Dispose();
                        dataSourceSet.Dispose();

                        this.loadedDirectories.Add(directory);
                    }
                    catch (Exception)
                    {
                        // TODO: custom error
                        return false;
                    }
                }

                lock (this.availableTables)
                {
                    this.availableTables.AddRange(newTables);
                }
                PluginLoaded?.Invoke(this, new PluginLoadedEventArgs(directory, newTables));
                return true;
            });
        }

        public async Task<bool> ProcessFile(string file, IEnumerable<IAvailableTable> enabledAvailableTables)
        {
            if (this.pluginSet == null)
            {
                return false;
            }

            using var dataSourceSet = DataSourceSet.Create(this.pluginSet, false);
            dataSourceSet.TryAddFile(file);

            var info = new EngineCreateInfo(dataSourceSet.AsReadOnly());

            var engine = Engine.Create(info);

            var tables = GetTablesFromGuid(engine, enabledAvailableTables.Select(table => table.Guid));
            foreach (var table in tables)
            {
                var enabledTable = engine.TryEnableTable(table);
            }

            if (!engine.EnabledTables.Any())
            {
                return false;
            }

            var result = engine.Process();
            await this.sessionsManager.CreateSessionAsync(new EngineSessionProvider(engine.EnabledTables, result));

            return true;
        }

        private List<TableDescriptor> GetTablesFromGuid(Engine engine, IEnumerable<Guid> guids)
        {
            return engine.AvailableTables.Where(table => guids.Contains(table.Guid)).ToList();
        }
    }
}
