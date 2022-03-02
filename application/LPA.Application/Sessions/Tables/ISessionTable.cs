using LPA.Application.Sessions.Tables.TableConfigs;
using LPA.Application.Sessions.Tables.View;

namespace LPA.Application.Sessions.Tables
{
    public interface ISessionTable
    {
        ISessionTableConfigurationsManager ConfigurationsManager { get; }

        Task<ISessionTableInfo> GetTableInfoAsync();

        Task<Guid> CreateViewAsync();

        Task<Guid> CreateViewAsync(Guid configId);

        Task CloseViewAsync(Guid viewId);

        Task<ISessionTableView> GetTableViewAsync(Guid viewId);

        Task<IEnumerable<ISessionTableView>> GetViewsAsync();
    }
}
