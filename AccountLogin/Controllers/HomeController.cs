using AccountLogin.Models;
using AccountShared.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace AccountLogin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;


        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var endPoint = "/api/Login/TestGet";
            //var endPoint = "/api/Login/TestPost";
            var endPoint = "/WeatherForecast";


            //// var json = await _httpClient.GetFromJsonAsync<object>(endPoint);

            // HttpResponseMessage response = await _httpClient.GetAsync(endPoint);
            // if (response.IsSuccessStatusCode)
            // {
            //     var content = await response.Content.ReadFromJsonAsync<object>();

            // }


            //var json = await _httpClient.PostAsJsonAsync<object>(endPoint, "hello");

            //if (json.IsSuccessStatusCode)
            //{
            //    var content = await json.Content.ReadAsStringAsync();
            //}


            //// for weatherforecast
            //var response = await _httpClient.GetAsync(endPoint);

            //if (response.IsSuccessStatusCode)
            //{
            //    var content = await response.Content.ReadAsStringAsync();
            //}





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
