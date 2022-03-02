using LPA.Application.Sessions.Provider;

namespace LPA.Application.Sessions.Tables.TableConfigs
{
    internal class SessionTableConfigurationsManager
        : ISessionTableConfigurationsManager
    {
        private readonly Guid tableId;
        private readonly ISessionProvider provider;

        private SessionTableConfigurationsManager(Guid tableId, ISessionProvider provider)
        {
            this.provider = provider;
            this.tableId = tableId;
        }

        internal static SessionTableConfigurationsManager Create(Guid tableId, ISessionProvider provider)
        {
            return new SessionTableConfigurationsManager(tableId, provider);
        }

        public async Task<IEnumerable<ITableConfiguration>> GetConfigurationsAsync()
        {
            return await this.provider.GetConfigurationsAsync(this.tableId);
        }
    }
}
