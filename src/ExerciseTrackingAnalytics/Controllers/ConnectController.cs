using Microsoft.AspNetCore.Mvc;

namespace ExerciseTrackingAnalytics.Controllers
{
    [Route("Connect")]
    public class ConnectController : Controller
    {
        private readonly ILogger<ConnectController> _logger;

        public ConnectController(ILogger<ConnectController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<ActionResult> Connect()
        {
            return Task.FromResult(View() as ActionResult);
        }
    }
}
