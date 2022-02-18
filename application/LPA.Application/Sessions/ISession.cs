namespace LPA.Application.Sessions
{
    public interface ISession
    {
        Guid Id { get; }

        Task<Guid[]> GetTablesAsync();

        // TODO: return more info than just col headers
        Task<string[]> GetTableConfigAsync(Guid tableId);

        Task<string[][]> GetTableDataAsync(Guid tableId);
    }
}
