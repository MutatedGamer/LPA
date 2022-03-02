using LPA.Application.Progress;
using LPA.Application.Sessions.Provider;
using LPA.Common;
using LPA.UI.Tags;

namespace LPA.Application.Sessions
{
    internal class SessionsManager
        : ISessionsManager
    {
        private readonly Dictionary<Guid, ISession> sessions = new();

        private readonly IUserInput userInput;
        private readonly IProgressManager progressManager;

        public SessionsManager(IUserInput userInput, IProgressManager progressManager)
        {
            this.userInput = userInput;
            this.progressManager = progressManager;
        }

        public async Task<Guid> CreateSessionAsync(ISessionProvider provider)
        {
            var guid = Guid.NewGuid();
            var session = await Session.Create(guid, provider, this.userInput, this.progressManager);

            lock (this.sessions)
            {
                this.sessions.Add(guid, session);
            }

            TagInvalidator.InvalidateTag(SessionsTag.Instance);

            return guid;
        }

        public Task<Guid[]> GetAvailableSessionsAsync()
        {
            lock (this.sessions)
            {
                return Task.FromResult(this.sessions.Keys.ToArray());
            }
        }

        public Task<ISession> GetSessionAsync(Guid sessionId)
        {
            lock (this.sessions)
            {
                if (!this.sessions.ContainsKey(sessionId))
                {
                    throw new InvalidOperationException($"A session with the given id {sessionId} does not exist.");
                }

                return Task.FromResult(this.sessions[sessionId]);
            }
        }
    }
}
