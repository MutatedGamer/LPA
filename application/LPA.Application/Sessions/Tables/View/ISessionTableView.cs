using LPA.Application.Sessions.Tables.Columns;

namespace LPA.Application.Sessions.Tables.View
{
    public interface ISessionTableView
    {
        Guid Id { get; }

        Task SetCurrentConfigurationAsync(Guid configId);

        Task<Guid> GetCurrentConfigurationIdAsync();

        Task<int> GetRowCountAsync();

        Task<IEnumerable<IColumn>> GetColumnsAsync();

        Task<string[][]> GetRowsAsync(int start, int count);

        Task ExportCsvAsync();
    }
}
