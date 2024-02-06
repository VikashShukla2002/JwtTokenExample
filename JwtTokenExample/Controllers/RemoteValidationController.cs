using AccountShared.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenExample.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RemoteValidationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RemoteValidationController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> IsUsernameAvailable(string username)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(username);

                if (userExists != null)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "user is already exist",
                        ErrorCode = 1
                    });
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Ok",
                        ErrorCode = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse
                {
                    Status = false,
                    Message = ex.Message,
                    ErrorCode = 2
                });
            }


        }
        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            try
            {
                var emailExists = await _userManager.FindByEmailAsync(email);
                if (emailExists != null)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Email is already exist",
                        ErrorCode = 1
                    });
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Ok",
                        ErrorCode = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse
                {
                    Status = false,
                    Message = ex.Message,
                    ErrorCode = 2
                });
            }
        }
    }
}
