using LPA.Application.EnabledTables;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnabledTablesController : ControllerBase
    {

        private readonly ILogger<EnabledTablesController> logger;
        private readonly IEnabledTablesManager enabledTablesManager;

        public EnabledTablesController(ILogger<EnabledTablesController> logger, IEnabledTablesManager enabledTablesManager)
        {
            this.logger = logger;
            this.enabledTablesManager = enabledTablesManager;
        }

        [HttpGet("{tableId}")]
        public async Task<ActionResult> GetIsEnabled(string tableId)
        {
            return Ok(await this.enabledTablesManager.IsEnabledAsync(Guid.Parse(tableId)));
        }

        [HttpPost("{tableId}")]
        public async Task<ActionResult> ToggleIsEnabled(string tableId)
        {
            await this.enabledTablesManager.ToggleEnabledAsync(Guid.Parse(tableId));
            return Ok();
        }

        [HttpPost("disableAll")]
        public async Task<ActionResult> DisabelAll()
        {
            await this.enabledTablesManager.DisableAll();
            return Ok();
        }
    }
}
