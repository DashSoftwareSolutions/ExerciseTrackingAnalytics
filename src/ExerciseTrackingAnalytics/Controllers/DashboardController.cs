using Microsoft.AspNetCore.Mvc;

namespace ExerciseTrackingAnalytics.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!(User?.Identity?.IsAuthenticated ?? false))
            {
                return Redirect("/");
            }

            return View();
        }
    }
}
