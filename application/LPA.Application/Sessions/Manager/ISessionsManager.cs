using LPA.Application.Sessions.Provider;

namespace LPA.Application.Sessions
{
    public interface ISessionsManager
    {
        /// <summary>
        /// Creates a new <see cref="ISession"/>.
        /// </summary>
        /// <param name="provider">
        /// The <see cref="ISessionProvider"/> that provides information necessary to
        /// construct the session.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/> of the created session.
        /// </returns>
        Task<Guid> CreateSessionAsync(ISessionProvider provider);

        /// <summary>
        /// Gets all available <see cref="ISession"/> <see cref="Guid"/>s.
        /// </summary>
        /// <returns>
        /// The <see cref="Guid"/>s that correspond to each available <see cref="ISession"/>. Any
        /// returned value can be used in <see cref="GetSessionAsync(Guid)"/> to retreive a <see cref="ISession"/>.
        /// </returns>
        Task<Guid[]> GetAvailableSessionsAsync();

        /// <summary>
        /// Gets the <see cref="ISession"/> that has a <see cref="ISession.Id"/> corresponding to the given
        /// <paramref name="sessionId"/>.
        /// </summary>
        /// <param name="sessionId">
        /// The <see cref="Guid"/> of the <see cref="ISession"/> to get.
        /// </param>
        /// <returns>
        /// The <see cref="ISession"/> associated with the given <paramref name="sessionId"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// A <see cref="ISession"/> with the given <paramref name="sessionId"/> does not exist.
        /// </exception>
        Task<ISession> GetSessionAsync(Guid sessionId);
    }
}
