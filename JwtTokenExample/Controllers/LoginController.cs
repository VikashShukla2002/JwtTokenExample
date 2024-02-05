using AccountShared.ViewModels;
using JwtTokenExample.DBContext;
using JwtTokenExample.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtTokenExample.Controllers
{


    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;


        private UserDb _context;

        public LoginController(IConfiguration config, UserDb context, UserManager<IdentityUser> userManager)
        {
            _configuration = config;
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserResponseModel login)
        {

            if (_context.Users.FirstOrDefault(a => a.Username == login.Username) is UserModel user)
            {
                if (!user.Password.Equals(login.Password))
                {
                    return Ok(new { status = false, message = "wrong password" }); ;
                }
                var tokenString = GenerateJSONWebToken(user);
                return Ok(new { status = true, token = tokenString });
            }
            else
            {
                return Unauthorized();
            }

        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            var userExists = await _userManager.FindByNameAsync(register.Username);

            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new IdentityUser()
            {
                UserName = register.Username,
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = result.Errors.First().Description
                });

            }


            return Ok(new Response { Status = "Success", Message = "User created successfully!" });


        }


        [AllowAnonymous]
        [HttpPost]

        public async Task<IActionResult> SignIn([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                //foreach (var userRole in userRoles)
                //{
                //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //}

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }




        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignUp([FromBody] UserResponseModel login)
        {



            var loginUser = new UserModel
            {
                Username = login.Username,
                Password = login.Password
            };
            _context.Users.Add(loginUser);
            _context.SaveChanges();
            return Ok(new { status = true, message = "Account has been created" }); ;

        }




        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

     

        [HttpPost]
        public IActionResult TestPost()
        {
            return Ok(new
            {
                message = "this is test post"
            });
        }
        
        [HttpGet]
        public IActionResult TestGet(IFormFile file)
        {
            return Ok(new
            {
                message = "this is test get"
            });
        }

    }
}
