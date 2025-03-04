using System.ComponentModel.DataAnnotations;

namespace GenericFanSite.Models.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(127)]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [StringLength(255)]
        [Compare("ConfirmPassword")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [StringLength(255)]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
