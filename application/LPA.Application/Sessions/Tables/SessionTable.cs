using LPA.Application.Sessions.Provider;
using LPA.Application.Sessions.Tables.TableConfigs;
using LPA.Application.Sessions.Tables.View;
using LPA.UI.Tags;

namespace LPA.Application.Sessions.Tables
{
    internal class SessionTable
        : ISessionTable
    {
        private readonly Guid sessionId;
        private readonly SessionTableViewsTag sessionTableViewsTag;
        private readonly Guid sessionTableId;

        private readonly ISessionProvider provider;

        private readonly Dictionary<Guid, ISessionTableView> views = new();

        private SessionTable(Guid sessionId, Guid sessionTableId, ISessionProvider provider, ISessionTableConfigurationsManager configurationsManager)
        {
            this.sessionId = sessionId;
            this.sessionTableViewsTag = new SessionTableViewsTag(sessionId);
            this.sessionTableId = sessionTableId;
            this.provider = provider;
            this.ConfigurationsManager = configurationsManager;
        }

        public ISessionTableConfigurationsManager ConfigurationsManager { get; private set; }

        internal static ISessionTable Create(Guid sessionId, Guid tableId, ISessionProvider provider)
        {
            var configManager = SessionTableConfigurationsManager.Create(tableId, provider);
            return new SessionTable(sessionId, tableId, provider, configManager);
        }

        public async Task<ISessionTableInfo> GetTableInfoAsync()
        {
            return await this.provider.GetTableInfoAsync(this.sessionTableId);
        }

        public async Task<Guid> CreateViewAsync()
        {
            return await CreateViewAsync(await this.provider.GetDefaultConfiguration(this.sessionTableId));
        }

        public async Task<Guid> CreateViewAsync(Guid configId)
        {
            var guid = Guid.NewGuid();
            var view = await SessionTableView.Create(guid, this.sessionTableId, configId, this.ConfigurationsManager, this.provider);

            lock (this.views)
            {
                this.views.Add(guid, view);
            }

            TagInvalidator.InvalidateTag(this.sessionTableViewsTag);

            return guid;
        }

        public async Task CloseViewAsync(Guid viewId)
        {
            lock (this.views)
            {
                this.views.Remove(viewId);
            }

            TagInvalidator.InvalidateTag(this.sessionTableViewsTag);
        }

        public Task<ISessionTableView> GetTableViewAsync(Guid viewId)
        {
            lock (this.views)
            {
                return Task.FromResult(this.views[viewId]);
            }
        }

        public Task<IEnumerable<ISessionTableView>> GetViewsAsync()
        {
            lock (this.views)
            {
                return Task.FromResult(this.views.Values.AsEnumerable());
            }
        }
    }
}
