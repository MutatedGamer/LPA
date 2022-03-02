using LPA.Application.Progress;
using LPA.UI.ResponseObjects.Progress;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("progress/{id:guid}")]
    public class ProgressController : ControllerBase
    {

        private readonly IProgressManager progressManager;

        public ProgressController(IProgressManager progressManager)
        {
            this.progressManager = progressManager;
        }

        [HttpGet]
        public async Task<ActionResult<ProgressState>> GetProgressState(Guid id)
        {
            var result = await this.progressManager.GetProgressStateAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]
        public async Task Cancel(Guid id)
        {
            await this.progressManager.CancelAsync(id);
        }
    }
}
