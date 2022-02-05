using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CounterController : ControllerBase
    {
        private static int currentCount = 0;

        private readonly ILogger<CounterController> _logger;

        public CounterController(ILogger<CounterController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(currentCount);
        }

        [HttpPost("increment")]
        public ActionResult Increment()
        {
            currentCount++;
            return Ok();
        }

        [HttpPost("decrement")]
        public ActionResult Decrement()
        {
            currentCount--;
            return Ok();
        }
    }
}