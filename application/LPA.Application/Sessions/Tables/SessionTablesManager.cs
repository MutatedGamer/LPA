using LPA.Application.Progress;
using LPA.Application.Sessions.Provider;
using LPA.Common;

namespace LPA.Application.Sessions.Tables
{
    internal class SessionTablesManager
        : ISessionTablesManager
    {
        private readonly Guid sessionId;
        private readonly ISessionProvider provider;

        private readonly Dictionary<Guid, ISessionTable> sessionTables = new();

        private SessionTablesManager(Guid sessionId, ISessionProvider provider)
        {
            this.sessionId = sessionId;
            this.provider = provider;
        }

        internal static async Task<ISessionTablesManager> Create(
            Guid sessionId,
            ISessionProvider provider,
            IUserInput userInput,
            IProgressManager progressManager)
        {
            var manager = new SessionTablesManager(sessionId, provider);

            foreach (var tableId in await provider.GetSessionTables())
            {
                var sessionTable = SessionTable.Create(sessionId, tableId, provider, userInput, progressManager);
                manager.sessionTables.Add(tableId, sessionTable);

                // TODO: don't do this.
                try
                {
                    if (await sessionTable.BuiltWithoutErrors())
                    {
                        await sessionTable.CreateViewAsync();
                    }
                }
                catch
                {
                    continue;
                }
            }

            return manager;
        }

        public Task<Guid[]> GetSessionTablesAsync()
        {
            return Task.FromResult(this.sessionTables.Keys.ToArray());
        }

        public Task<ISessionTable> GetSessionTableAsync(Guid tableId)
        {
            if (!this.sessionTables.ContainsKey(tableId))
            {
                throw new InvalidOperationException($"Table with id {tableId} not found.");
            }

            return Task.FromResult(this.sessionTables[tableId]);
        }
    }
}
