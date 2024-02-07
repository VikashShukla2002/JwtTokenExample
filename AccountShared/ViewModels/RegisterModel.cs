using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace AccountShared.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        [Remote("IsUsernameAvailable", "Account")]
        public string? Username { get; set; }

        [EmailAddress]
        [Remote("IsEmailAvailable", "Account")]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
