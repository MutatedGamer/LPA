namespace LPA.Application.Sessions.Tables.TableConfigs
{
    public interface ISessionTableConfigurationsManager
    {
        Task<IEnumerable<ITableConfiguration>> GetConfigurationsAsync();
    }
}
