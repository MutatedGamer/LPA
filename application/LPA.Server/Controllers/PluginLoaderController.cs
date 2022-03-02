using LPA.Application.PluginLoading;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("pluginLoader")]
    public class PluginLoaderController : ControllerBase
    {

        private readonly IPluginLoader pluginLoader;

        public PluginLoaderController(IPluginLoader pluginLoader)
        {
            this.pluginLoader = pluginLoader;
        }

        [HttpPost]
        public async Task LoadDirectory()
        {
            await this.pluginLoader.LoadPluginAsync();
        }
    }
}
