using LPA.Application.Sessions.Provider;

namespace LPA.Application.Sessions
{
    public interface ISessionsManager
    {
        Task<Guid> CreateSessionAsync(ISessionProvider provider);

        Task<Guid[]> GetAvailableSessionsAsync();

        Task<ISession> GetSessionAsync(Guid sessionId);
    }
}
