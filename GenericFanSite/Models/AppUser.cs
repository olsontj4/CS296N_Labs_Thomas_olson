using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericFanSite.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime SignUpDate { get; set; }
        [NotMapped]
        public IList<string>? RoleNames { get; set; }
    }
}