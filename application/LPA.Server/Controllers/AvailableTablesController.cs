using LPA.Application.AvailableTables;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvailableTablesController : ControllerBase
    {

        private readonly ILogger<AvailableTablesController> logger;
        private readonly IAvailableTablesManager availableTablesManager;

        public AvailableTablesController(ILogger<AvailableTablesController> logger, IAvailableTablesManager pluginLoader)
        {
            this.logger = logger;
            this.availableTablesManager = pluginLoader;
        }

        [HttpGet]
        public async Task<ActionResult> GetAvailableTables()
        {
            var result = await this.availableTablesManager.GetAvailableTablesAsync();
            return Ok(result);
        }
    }
}
