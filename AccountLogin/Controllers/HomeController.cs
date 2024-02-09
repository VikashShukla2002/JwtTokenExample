using AccountLogin.Models;
using AccountShared.ViewModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
//using Google.Apis.Discovery.v3;
//using Google.Apis.Discovery.v1.Data;
//using Google.Apis.Services;

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





        public async Task<IActionResult> Upload(IFormFile file)
        {


            if (file != null && file.Length > 0)
            {
                // Create the service.

                //var filePath = Path.Combine("uploadedFiles", file.FileName);
                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await file.CopyToAsync(stream);
                //}
                //
                try
                {
                    string[] scopes = new string[] { DriveService.Scope.Drive,
                               DriveService.Scope.DriveFile,};

                    var clientId = "275545007442-91j8alp2eleai9sfoi7v2vl5gnui9o7o.apps.googleusercontent.com";      // From https://console.developers.google.com  
                    var clientSecret = "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";

                    var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret
                    }, scopes,
                    Environment.UserName, CancellationToken.None, new FileDataStore("MyAppsToken"));
                    DriveService service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "VikashLoginWithGoogle",

                    });
                    service.HttpClient.Timeout = TimeSpan.FromMinutes(100);

                    //uploadFile(service, "" )
                }
                catch (Exception ex)
                {


                }




                return RedirectToAction("Privacy");
            }
            else
            {

                return View("Error");
            }
        }

        public Google.Apis.Drive.v3.Data.File uploadFile(DriveService _service, string _uploadFile, string _parent, IFormFile file, string _descrp = "Uploaded with .NET!" )
        {
            if (System.IO.File.Exists(_uploadFile))
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                body.Name = System.IO.Path.GetFileName(_uploadFile);
                body.Description = _descrp;
                body.MimeType = file.ContentType;
                // body.Parents = new List<string> { _parent };// UN comment if you want to upload to a folder(ID of parent folder need to be send as paramter in above method)
                byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.CreateMediaUpload request = _service.Files.Create(body, stream, file.ContentType);
                    request.SupportsTeamDrives = true;
                  
                    request.Upload();
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadFile(MediaFile file)
        //{
        //    var service = new DiscoveryService(new BaseClientService.Initializer
        //    {
        //        ApplicationName = "Discovery Sample",
        //        ApiKey = "",
        //    });

        //    // Run the request.
        //    Console.WriteLine("Executing a list request...");
        //    var result = await service.Apis.List().ExecuteAsync();

        //    // Display the results.
        //    if (result.Items != null)
        //    {
        //        foreach (DirectoryList.ItemsData api in result.Items)
        //        {
        //            Console.WriteLine(api.Id + " - " + api.Title);
        //        }
        //    }


        //    return View();
        //}





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
