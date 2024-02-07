using AccountShared.ViewModels;
using JwtTokenExample.DBContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace AccountLogin.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {

            if (ModelState.IsValid)
            {
                var endpoint = "api/Login/SignIn";
                var response = await _httpClient.PostAsJsonAsync(endpoint, loginModel);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

                    var claims = ParseClaimsFromJwt(tokenResponse!.Token);
                    var claimsList = claims?.ToList();

                    var claimsIdentity = new ClaimsIdentity(
                    claimsList, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties { };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    Response.Cookies.Append("Token", tokenResponse.Token);

                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.Token);
                    return RedirectToAction("Index", "Home");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized) // 401 Unauthorized
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View(loginModel);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                    return View(loginModel);
                }

            }

            return View(loginModel);
        }

        // For decode token
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private static IEnumerable<Claim>? ParseClaimsFromJwt(string jwtToken)
        {
            var claims = new List<Claim>();
            var payload = jwtToken.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            claims.AddRange(keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
            return claims;
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
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
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(registerModel);
        }

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
    }
}
