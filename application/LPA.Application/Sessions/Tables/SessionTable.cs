using LPA.Application.Progress;
using LPA.Application.Sessions.Provider;
using LPA.Application.Sessions.Tables.TableConfigs;
using LPA.Application.Sessions.Tables.View;
using LPA.Common;
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
        private readonly IUserInput userInput;
        private readonly IProgressManager progressManager;

        private SessionTable(
            Guid sessionId,
            Guid sessionTableId,
            ISessionProvider provider,
            ISessionTableConfigurationsManager configurationsManager,
            IUserInput userInput,
            IProgressManager progressManager)
        {
            this.sessionId = sessionId;
            this.sessionTableViewsTag = new SessionTableViewsTag(sessionId);
            this.sessionTableId = sessionTableId;
            this.provider = provider;
            this.ConfigurationsManager = configurationsManager;
            this.userInput = userInput;
            this.progressManager = progressManager;
        }

        public ISessionTableConfigurationsManager ConfigurationsManager { get; private set; }

        internal static ISessionTable Create(
            Guid sessionId,
            Guid tableId,
            ISessionProvider provider,
            IUserInput userInput,
            IProgressManager progressManager)
        {
            var configManager = SessionTableConfigurationsManager.Create(tableId, provider);
            return new SessionTable(sessionId, tableId, provider, configManager, userInput, progressManager);
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
            var view = await SessionTableView.Create(guid, this.sessionTableId, configId, this.ConfigurationsManager, this.provider, this.userInput, this.progressManager);

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

        public async Task<bool> BuiltWithoutErrors()
        {
            return await this.provider.BuiltWithoutErrors(this.sessionTableId);
        }
    }
}
