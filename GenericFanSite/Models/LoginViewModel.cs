using System.ComponentModel.DataAnnotations;

namespace GenericFanSite.Models
{
    public class LoginViewModel
    {
        public string? ReturnUrl { get; internal set; }
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(127)]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(255)]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
