using LPA.Application.Progress;
using LPA.Application.Sessions.Provider;
using LPA.Application.Sessions.Tables;
using LPA.Common;

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

        internal static async Task<Session> Create(
            Guid sessionId,
            ISessionProvider provider,
            IUserInput userInput,
            IProgressManager progressManager)
        {
            var tableManager = await SessionTablesManager.Create(sessionId, provider, userInput, progressManager);
            var session = new Session(sessionId, provider, tableManager);

            return session;
        }
    }
}
