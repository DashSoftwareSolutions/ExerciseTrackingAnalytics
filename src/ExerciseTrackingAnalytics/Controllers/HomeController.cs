using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExerciseTrackingAnalytics.ViewModels;

namespace ExerciseTrackingAnalytics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return Redirect("/dashboard");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error([FromQuery] int? statusCode = null)
        {
            var errorMessage = ControllerContext.HttpContext.Items["ValidationErrorMessage"]?.ToString();
            
            return View(
                new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    StatusCode = statusCode
                });
        }
    }
}