using LPA.Application.Sessions;
using LPA.Server.Utils;
using LPA.UI.ResponseObjects.Tables;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("sessions/{sessionId:guid}/sessionTables/{sessionTableId:guid}/views/{viewId:guid}")]
    public class SessionTableViewsController : ControllerBase
    {

        private readonly ISessionsManager sessionsManager;

        public SessionTableViewsController(ISessionsManager sessionsManager)
        {
            this.sessionsManager = sessionsManager;
        }

        [HttpPost("close")]
        public async Task<IActionResult> Close(Guid sessionId, Guid sessionTableId, Guid viewId)
        {
            var table = await this.sessionsManager.GetSessionTable(sessionId, sessionTableId);
            await table.CloseViewAsync(viewId);

            return Ok();
        }

        [HttpGet("config")]
        public async Task<ActionResult<Guid>> GetCurrentTableConfig(Guid sessionId, Guid sessionTableId, Guid viewId)
        {
            var view = await this.sessionsManager.GetSessionTableView(sessionId, sessionTableId, viewId);

            return Ok(await view.GetCurrentConfigurationIdAsync());
        }

        [HttpPost("config/{configId:guid}")]
        public async Task<ActionResult<Guid>> SetCurrentTableConfig(Guid sessionId, Guid sessionTableId, Guid viewId, Guid configId)
        {
            var view = await this.sessionsManager.GetSessionTableView(sessionId, sessionTableId, viewId);

            await view.SetCurrentConfigurationAsync(configId);

            return Ok();
        }

        [HttpGet("columns")]
        public async Task<IEnumerable<Column>> GetColumns(Guid sessionId, Guid sessionTableId, Guid viewId)
        {
            var view = await this.sessionsManager.GetSessionTableView(sessionId, sessionTableId, viewId);

            var columns = await view.GetColumnsAsync();

            return columns.Select(column => new Column()
            {
                IsGraph = column.IsGraph,
                IsPivot = column.IsPivot,
                IsLeftFrozen = column.IsLeftFrozen,
                IsRightFrozen = column.IsRightFrozen,
                Name = column.Name,
                SessionTableColumnGuid = column.SessionTableColumnGuid
            });
        }

        [HttpGet("rowCount")]
        public async Task<ActionResult<uint>> GetRowCount(Guid sessionId, Guid sessionTableId, Guid viewId)
        {
            var view = await this.sessionsManager.GetSessionTableView(sessionId, sessionTableId, viewId);

            return Ok(await view.GetRowCountAsync());
        }

        // TODO: use params or something idk
        [HttpGet("rows/{start:int}/{count:int}")]
        public async Task<ActionResult<string[][]>> GetRows(Guid sessionId, Guid sessionTableId, Guid viewId, int start, int count)
        {
            var view = await this.sessionsManager.GetSessionTableView(sessionId, sessionTableId, viewId);

            return Ok(await view.GetRowsAsync(start, count));
        }
    }
}
