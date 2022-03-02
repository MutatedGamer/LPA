using LPA.Application.Sessions;
using LPA.Server.Utils;
using LPA.UI.ResponseObjects.SessionTables;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("sessions/{sessionId:guid}/sessionTables/{sessionTableId:guid}")]
    public class SessionTablesController : ControllerBase
    {

        private readonly ISessionsManager sessionsManager;

        public SessionTablesController(ISessionsManager sessionsManager)
        {
            this.sessionsManager = sessionsManager;
        }

        [HttpGet("info")]
        public async Task<SessionTableInfo> GetTableInfo(Guid sessionId, Guid sessionTableId)
        {
            var table = await this.sessionsManager.GetSessionTable(sessionId, sessionTableId);
            var info = await table.GetTableInfoAsync();

            return new SessionTableInfo()
            {
                Name = info.Name,
                Category = info.Category,
            };
        }

        [HttpGet("configs")]
        public async Task<IEnumerable<SessionTableConfig>> GetTableConfigs(Guid sessionId, Guid sessionTableId)
        {
            var table = await this.sessionsManager.GetSessionTable(sessionId, sessionTableId);

            var response = (await table.ConfigurationsManager.GetConfigurationsAsync()).Select(config => new SessionTableConfig()
            {
                Name = config.Name,
                Id = config.Id
            });

            return response;
        }
    }
}
