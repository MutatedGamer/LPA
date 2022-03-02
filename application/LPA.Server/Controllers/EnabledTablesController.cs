using LPA.Application.EnabledTables;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("enabledTables")]
    public class EnabledTablesController : ControllerBase
    {

        private readonly IEnabledTablesManager enabledTablesManager;

        public EnabledTablesController(IEnabledTablesManager enabledTablesManager)
        {
            this.enabledTablesManager = enabledTablesManager;
        }

        [HttpGet("{tableId:guid}")]
        public async Task<bool> GetIsEnabled(Guid tableId)
        {
            return await this.enabledTablesManager.IsEnabledAsync(tableId);
        }

        [HttpPost("{tableId:guid}")]
        public async Task ToggleIsEnabled(Guid tableId)
        {
            await this.enabledTablesManager.ToggleEnabledAsync(tableId);
            return;
        }

        [HttpPost("disableAll")]
        public async Task DisabelAll()
        {
            await this.enabledTablesManager.DisableAll();
        }
    }
}
