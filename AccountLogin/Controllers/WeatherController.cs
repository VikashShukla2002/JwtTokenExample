using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountLogin.Controllers
{
    [Authorize]
    public class WeatherController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
