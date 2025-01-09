using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GenericFanSite.Models
{
    public class AppUser : IdentityUser
    {
        /*public int AppUserId { get; set; }
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(25, ErrorMessage = "Max length of name is 25 characters.")]
        public string? Name { get; set; }*/
        public DateTime SignUpDate { get; set; }
    }
}