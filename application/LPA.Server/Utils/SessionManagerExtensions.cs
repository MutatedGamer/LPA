using LPA.Application.Sessions;
using LPA.Application.Sessions.Tables;
using LPA.Application.Sessions.Tables.View;

namespace LPA.Server.Utils
{
    public static class SessionManagerExtensions
    {
        public static async Task<ISessionTable> GetSessionTable(this ISessionsManager manager, Guid sessionId, Guid sessionTableId)
        {
            var session = await manager.GetSessionAsync(sessionId);
            return await session.TablesManager.GetSessionTableAsync(sessionTableId);
        }

        public static async Task<ISessionTableView> GetSessionTableView(this ISessionsManager manager, Guid sessionId, Guid sessionTableId, Guid viewId)
        {
            var session = await manager.GetSessionAsync(sessionId);
            var sessionTable = await session.TablesManager.GetSessionTableAsync(sessionTableId);

            return await sessionTable.GetTableViewAsync(viewId);
        }
    }
}
