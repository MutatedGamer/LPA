using LPA.Application.PluginLoading;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PluginLoaderController : ControllerBase
    {

        private readonly ILogger<PluginLoaderController> logger;
        private readonly IPluginLoader pluginLoader;

        public PluginLoaderController(ILogger<PluginLoaderController> logger, IPluginLoader pluginLoader)
        {
            this.logger = logger;
            this.pluginLoader = pluginLoader;
        }

        [HttpPost]
        public async Task<ActionResult> LoadDirectory()
        {
            await this.pluginLoader.LoadPluginAsync();
            return Ok();
        }
    }
}
