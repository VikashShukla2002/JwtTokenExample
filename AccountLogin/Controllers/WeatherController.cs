using AccountShared.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountLogin.Controllers
{
    [Authorize]
    public class WeatherController : Controller
    {
        private readonly HttpClient _httpClient;

        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Weather()
        {
            var endpoint = "/WeatherForecast";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Request.Cookies["Token"]);
            var response = await _httpClient.GetFromJsonAsync<List<WeatherForecast>>(endpoint);

            return View(response);
        }
    }
}
