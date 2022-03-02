namespace LPA.Application.Sessions.Tables
{
    public interface ISessionTablesManager
    {
        /// <summary>
        /// Gets all valid <see cref="Guid"/>s for tables
        /// </summary>
        /// <returns>
        /// TODO - document
        /// </returns>
        Task<Guid[]> GetSessionTablesAsync();

        // TODO - document
        Task<ISessionTable> GetSessionTableAsync(Guid tableId);
    }
}
