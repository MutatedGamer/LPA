using LPA.Application.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("sessions")]
    public class SessionsController : ControllerBase
    {

        private readonly ISessionsManager sessionsManager;

        public SessionsController(ISessionsManager sessionsManager)
        {
            this.sessionsManager = sessionsManager;
        }

        [HttpGet]
        public async Task<IEnumerable<Guid>> GetSessions()
        {
            return await this.sessionsManager.GetAvailableSessionsAsync();
        }
    }
}
