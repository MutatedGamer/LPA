using LPA.Application.Sessions.Tables;
using LPA.Application.Sessions.Tables.TableConfigs;

namespace LPA.Application.Sessions.Provider
{
    public interface ISessionProvider
    {
        Task<Guid[]> GetSessionTables();

        Task<ISessionTableInfo> GetTableInfoAsync(Guid tableId);

        Task<ITableConfiguration[]> GetConfigurationsAsync(Guid tableId);

        Task<ITableConfiguration?> GetConfigurationAsync(Guid tableId, Guid configId);

        Task<Guid> GetDefaultConfiguration(Guid tableId);

        Task<int> GetRowCountAsync(Guid tableId);

        Task<string[][]> GetRowsAsync(Guid tableId, IEnumerable<Guid> columns, int start, int count);
    }
}
