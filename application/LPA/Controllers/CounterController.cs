using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;

namespace LPA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CounterController : ControllerBase
    {
        private static int currentCount = 0;

        private readonly ILogger<CounterController> _logger;
        private readonly IpcMain ipcMain;
        private readonly WindowManager windowManager;

        public CounterController(ILogger<CounterController> logger, IpcMain ipcMain, WindowManager windowManager)
        {
            this._logger = logger;
            this.ipcMain = ipcMain;
            this.windowManager = windowManager;
        }

        public static void StartRandomizeLoop()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    // Wait between 1 and 8s
                    var delayTime = Random.Shared.Next(1000, 8000);
                    await Task.Delay(delayTime);

                    currentCount = Random.Shared.Next(0, 5000);

                    // Check for window to be loaded
                    if (!Electron.WindowManager.BrowserWindows.Any())
                    {
                        continue;
                    }

                    // NOTE: We only have one window open for now, so we can just
                    // grab the .First() window. We'll revisit this later.
                    var window = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(
                        window,
                        "invalidate",
                        "Count");
                }
            });
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