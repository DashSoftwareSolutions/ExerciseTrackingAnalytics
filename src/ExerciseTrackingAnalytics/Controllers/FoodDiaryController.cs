using Microsoft.AspNetCore.Mvc;

namespace ExerciseTrackingAnalytics.Controllers
{
    public class FoodDiaryController : Controller
    {
        [Route("/food-diary")]
        [Route("/food-diary/{date}")]
        public IActionResult Index([FromRoute] string? date = null)
        {
            if (!(User?.Identity?.IsAuthenticated ?? false))
            {
                return Redirect("/");
            }

            DateOnly parsedDate;

            if (!string.IsNullOrEmpty(date))
            {
                if (!DateOnly.TryParse(date, out parsedDate))
                {
                    HttpContext.Items["ValidationErrorMessage"] = $"'date' parameter has an invalid value '{date}'.  It is expected to be a date in YYYY-MM-DD format (ISO 8601).";
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            else
            {
                parsedDate = DateOnly.FromDateTime(DateTime.Today);
            }

            ViewBag.Date = parsedDate;
            return View();
        }
    }
}
