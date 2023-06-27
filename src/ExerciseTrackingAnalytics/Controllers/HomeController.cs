using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ExerciseTrackingAnalytics.Models;
using StravaAuthentication = ExerciseTrackingAnalytics.Security.Authentication.Strava.Constants;

namespace ExerciseTrackingAnalytics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var accessToken = await ControllerContext.HttpContext.GetTokenAsync(StravaAuthentication.AuthenticationScheme, "access_token");
                var refreshToken = await ControllerContext.HttpContext.GetTokenAsync(StravaAuthentication.AuthenticationScheme, "refresh_token");
                ViewBag.AccessToken = accessToken;
                ViewBag.RefreshToken = refreshToken;
            }

            var authResult = await HttpContext.AuthenticateAsync();
            if (authResult != null)
            {
                ViewBag.AuthProperties = authResult.Properties?.Items;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}