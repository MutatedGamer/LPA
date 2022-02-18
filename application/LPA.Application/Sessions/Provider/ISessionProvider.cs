namespace LPA.Application.Sessions.Provider
{
    public interface ISessionProvider
    {
        Task<(string name, Guid guid)[]> GetDefaultConfigColumnsAsync(Guid tableId);

        Task<string[][]> GetTableDataAsync(Guid tableId);

        Task IsTableDataAvailable(Guid tableGuid);

        Task<Guid[]> GetSessionTables();
    }
}
