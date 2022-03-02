using LPA.Application.Sessions.Provider;
using LPA.Application.Sessions.Tables;

namespace LPA.Application.Sessions
{
    internal class Session
        : ISession
    {
        private readonly ISessionProvider provider;

        public Guid Id { get; }

        public ISessionTablesManager TablesManager { get; private set; }

        private Session(Guid id, ISessionProvider provider, ISessionTablesManager tableManager)
        {
            this.provider = provider;
            this.Id = id;
            this.TablesManager = tableManager;
        }

        internal static async Task<Session> Create(Guid sessionId, ISessionProvider provider)
        {
            var tableManager = await SessionTablesManager.Create(sessionId, provider);
            var session = new Session(sessionId, provider, tableManager);

            return session;
        }
    }
}
