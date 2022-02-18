using LPA.Application.Sessions.Provider;

namespace LPA.Application.Sessions
{
    internal class Session
        : ISession
    {
        private readonly Guid id;

        private readonly ISessionProvider provider;

        private readonly HashSet<Guid> builtTables = new();

        public Session(Guid id, ISessionProvider provider)
        {
            this.id = id;
            this.provider = provider;
        }

        public Guid Id => this.id;

        public async Task<string[]> GetTableConfigAsync(Guid tableId)
        {
            return (await this.provider.GetDefaultConfigColumnsAsync(tableId)).Select(col => col.name).ToArray();
        }

        public Task<string[][]> GetTableDataAsync(Guid tableId)
        {
            return this.provider.GetTableDataAsync(tableId);
        }

        public async Task<Guid[]> GetTablesAsync()
        {
            return await this.provider.GetSessionTables();
        }
    }
}
