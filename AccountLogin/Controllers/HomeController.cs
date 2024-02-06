using AccountLogin.Models;
using AccountShared.ViewModels;
using Microsoft.AspNetCore.Authentication;
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

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _httpClient = httpClient;
            _userManager = userManager;
            _signInManager = signInManager;
        }

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


        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {

            if (ModelState.IsValid)
            {

                var endpoint = "api/Login/Register";
                var response = await _httpClient.PostAsJsonAsync(endpoint, registerModel);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("Index");

                }
            }


            return View(registerModel);

        }

        //[HttpGet]
        public async Task<IActionResult> IsUsernameAvailable(string username)
        {
            var endpoint = $"/RemoteValidation/IsUsernameAvailable?username={Uri.EscapeDataString(username)}";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse>(endpoint);

                if (response.Status && response.ErrorCode > 0)
                {
                    return Json(data: response.Message);
                }
                else
                {
                    return Json(data: true);
                }
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message);
            }
        }
        //[HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {

            var endpoint = $"/RemoteValidation/IsEmailAvailable?email={Uri.EscapeDataString(email)}";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse>(endpoint);
                if (response.Status && response.ErrorCode > 0)
                {
                    return Json(data: response.Message);
                }
                else
                {
                    return Json(data: true);
                }
            }
            catch (Exception ex)
            {
                return Json(data: ex.Message);
            }
        }

        public IActionResult SignIn()
        {
            return View(new LoginModel());
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var endpoint = "api/Login/SignIn";
                var response = await _httpClient.PostAsJsonAsync(endpoint, loginModel);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                    if (tokenResponse != null)
                    {
                        // Store the token in a secure manner (e.g., using cookie authentication)
                        // For example, you could use ASP.NET Core authentication mechanisms:
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, loginModel.Username),
                            new Claim(JwtRegisteredClaimNames.Jti, tokenResponse.Token)
                        };

                        var identity = new ClaimsIdentity(claims, "Bearer");
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(principal);
                        

                        // Redirect to the desired page after successful login
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(loginModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Home");
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
