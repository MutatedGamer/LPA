using LPA.Application.Sessions;
using LPA.UI.ResponseObjects.SessionTables;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("sessions/{sessionId:guid}")]
    public class SessionController : ControllerBase
    {

        private readonly ISessionsManager sessionsManager;

        public SessionController(ISessionsManager sessionsManager)
        {
            this.sessionsManager = sessionsManager;
        }

        [HttpGet("sessionTables")]
        public async Task<IEnumerable<Guid>> GetSessionTables(Guid sessionId)
        {
            var session = await this.sessionsManager.GetSessionAsync(sessionId);

            return await session.TablesManager.GetSessionTablesAsync();
        }

        [HttpGet("sessionTableViews")]
        public async Task<IEnumerable<SessionTableViewIdentifier>> GetSessionTableViews(Guid sessionId)
        {
            var session = await this.sessionsManager.GetSessionAsync(sessionId);

            var result = new List<SessionTableViewIdentifier>();

            var sessionTables = await session.TablesManager.GetSessionTablesAsync();
            foreach (var sessionTable in sessionTables)
            {
                foreach (var view in await (await session.TablesManager.GetSessionTableAsync(sessionTable)).GetViewsAsync())
                {
                    result.Add(
                        new SessionTableViewIdentifier()
                        {
                            SessionTableId = sessionTable,
                            SessionTableViewId = view.Id,
                        });
                }
            }

            return result;
        }
    }
}
