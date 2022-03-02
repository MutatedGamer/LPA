using LPA.Application.AvailableTables;
using LPA.UI.ResponseObjects.AvailableTables;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("availableTables")]
    public class AvailableTablesController : ControllerBase
    {
        private readonly IAvailableTablesManager availableTablesManager;

        public AvailableTablesController(IAvailableTablesManager pluginLoader)
        {
            this.availableTablesManager = pluginLoader;
        }

        [HttpGet]
        public async Task<IEnumerable<AvailableTable>> GetAvailableTables()
        {
            var result = await this.availableTablesManager.GetAvailableTablesAsync();
            return result.Select(x => new AvailableTable()
            {
                Guid = x.Guid,
                Name = x.Name,
                PluginDirectory = x.PluginDirectory,
                Description = x.Description,
                Category = x.Category,
            });
        }
    }
}
