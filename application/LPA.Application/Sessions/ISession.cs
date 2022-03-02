using LPA.Application.Sessions.Tables;

namespace LPA.Application.Sessions
{
    public interface ISession
    {
        Guid Id { get; }

        ISessionTablesManager TablesManager { get; }
    }
}
