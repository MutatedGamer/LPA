using LPA.Application.Sessions.Provider;
using LPA.Application.Sessions.Tables.Columns;
using LPA.Application.Sessions.Tables.TableConfigs;
using LPA.UI.Tags;

namespace LPA.Application.Sessions.Tables.View
{
    internal class SessionTableView
        : ISessionTableView
    {
        private readonly Guid sessionTableId;
        private readonly ISessionTableConfigurationsManager configManager;
        private readonly ISessionProvider provider;

        private ITableConfiguration? currentConfig;

        private SessionTableView(
            Guid id,
            Guid sessionTableId,
            ISessionTableConfigurationsManager configurationsManager,
            ISessionProvider provider)
        {
            this.tableDataTag = new SessionTableViewDataTag(id);
            this.columnsTag = new SessionTableViewColumnsTag(id);

            this.Id = id;
            this.sessionTableId = sessionTableId;
            this.configManager = configurationsManager;
            this.provider = provider;
        }

        private readonly SessionTableViewDataTag tableDataTag;
        private readonly SessionTableViewColumnsTag columnsTag;

        public Guid Id { get; private set; }

        public static async Task<SessionTableView> Create(
            Guid id,
            Guid sessionTableId,
            Guid configId,
            ISessionTableConfigurationsManager configurationsManager,
            ISessionProvider provider)
        {
            var view = new SessionTableView(id, sessionTableId, configurationsManager, provider);
            await view.SetCurrentConfigurationAsync(configId);
            return view;
        }

        public Task<Guid> GetCurrentConfigurationIdAsync()
        {
            return Task.FromResult(this.currentConfig?.Id ?? Guid.Empty);
        }

        public async Task SetCurrentConfigurationAsync(Guid configId)
        {
            this.currentConfig = await this.provider.GetConfigurationAsync(this.sessionTableId, configId);

            TagInvalidator.InvalidateTag(this.tableDataTag);
            TagInvalidator.InvalidateTag(this.columnsTag);
        }

        public async Task<uint> GetRowCountAsync()
        {
            return await this.provider.GetRowCountAsync(this.sessionTableId);
        }

        public Task<IEnumerable<IColumn>> GetColumnsAsync()
        {
            // TODO: null check?
            return Task.FromResult(GetVisibleColumns());
        }

        public async Task<string[][]> GetRowsAsync(int start, int count)
        {
            return await this.provider.GetRowsAsync(this.sessionTableId, GetVisibleColumns().Select(col => col.SessionTableColumnGuid).ToList(), start, count);
        }

        private IEnumerable<IColumn> GetVisibleColumns()
        {
            return this.currentConfig.Columns.Where(col => col.IsVisible);
        }
    }
}
