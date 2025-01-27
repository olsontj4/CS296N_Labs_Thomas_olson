using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GenericFanSite.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime SignUpDate { get; set; }
    }
}