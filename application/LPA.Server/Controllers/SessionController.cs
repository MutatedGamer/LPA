using LPA.Application.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("[controller]/{sessionId}")]
    public class SessionController : ControllerBase
    {

        private readonly ILogger<SessionController> logger;
        private readonly ISessionsManager sessionsManager;

        public SessionController(ILogger<SessionController> logger, ISessionsManager sessionsManager)
        {
            this.logger = logger;
            this.sessionsManager = sessionsManager;
        }

        [HttpGet("tables")]
        public async Task<ActionResult> GetTables(string sessionId)
        {
            var id = Guid.Parse(sessionId);
            var session = await this.sessionsManager.GetSessionAsync(id);

            var result = await session.GetTablesAsync();
            return Ok(result);
        }

        [HttpGet("tables/{tableId}/rows")]
        public async Task<ActionResult> GetTableRows(string sessionId, string tableId)
        {
            var sessionGuid = Guid.Parse(sessionId);
            var session = await this.sessionsManager.GetSessionAsync(sessionGuid);

            var tableGuid = Guid.Parse(tableId);
            var result = await session.GetTableDataAsync(tableGuid);
            return Ok(result);
        }

        [HttpGet("tables/{tableId}/config")]
        public async Task<ActionResult> GetTableConfig(string sessionId, string tableId)
        {
            var id = Guid.Parse(sessionId);
            var session = await this.sessionsManager.GetSessionAsync(id);

            var tableGuid = Guid.Parse(tableId);
            var result = await session.GetTableConfigAsync(tableGuid);
            return Ok(result);
        }
    }
}
