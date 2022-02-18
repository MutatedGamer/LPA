using LPA.Application.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionsController : ControllerBase
    {

        private readonly ILogger<SessionsController> logger;
        private readonly ISessionsManager sessionsManager;

        public SessionsController(ILogger<SessionsController> logger, ISessionsManager sessionsManager)
        {
            this.logger = logger;
            this.sessionsManager = sessionsManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetSessions()
        {
            var result = await this.sessionsManager.GetAvailableSessionsAsync();
            return Ok(result);
        }
    }
}
