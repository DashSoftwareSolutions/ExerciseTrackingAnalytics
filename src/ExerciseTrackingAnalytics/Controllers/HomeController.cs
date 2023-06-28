using System.Diagnostics;
using System.Globalization;
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

        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var accessToken = User.FindFirst($"{StravaAuthentication.AuthenticationScheme.ToLower()}_access_token")?.Value;
                var refreshToken = User.FindFirst($"{StravaAuthentication.AuthenticationScheme.ToLower()}_refresh_token")?.Value;
                var accessTokenExpirationString = User.FindFirst($"{StravaAuthentication.AuthenticationScheme.ToLower()}_access_token_expires_at")?.Value;

                ViewBag.AccessToken = accessToken;
                ViewBag.RefreshToken = refreshToken;

                if (accessTokenExpirationString != null &&
                    DateTime.TryParse(
                        accessTokenExpirationString,
                        CultureInfo.CurrentCulture,
                        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                        out var accessTokenExpiration))
                {
                    ViewBag.AccessTokenExpiration = accessTokenExpiration.ToString("o");
                }
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