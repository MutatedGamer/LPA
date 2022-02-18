using LPA.Application.Sessions.Provider;
using LPA.UI.Tags;

namespace LPA.Application.Sessions
{
    internal class SessionsManager
        : ISessionsManager
    {
        private readonly Dictionary<Guid, ISession> sessions = new();

        public Task<Guid> CreateSessionAsync(ISessionProvider provider)
        {
            var guid = Guid.NewGuid();
            var session = new Session(guid, provider);

            lock (this.sessions)
            {
                this.sessions.Add(guid, session);
            }

            TagInvalidator.InvalidateTag(SessionsTag.Instance);

            return Task.FromResult(guid);
        }

        public async Task<Guid[]> GetAvailableSessionsAsync()
        {
            // REMOVE THIS. For testing :)
            await Task.Delay(2000);

            lock (this.sessions)
            {
                return this.sessions.Keys.ToArray();
            }
        }

        public Task<ISession> GetSessionAsync(Guid sessionId)
        {
            lock (this.sessions)
            {
                return Task.FromResult(this.sessions[sessionId]);
            }
        }
    }
}
