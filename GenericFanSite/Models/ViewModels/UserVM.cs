using Microsoft.AspNetCore.Identity;

namespace GenericFanSite.Models.ViewModels
{
    public class UserVM
    {
        public IEnumerable<AppUser>? AppUsers { get; set; }
        public IEnumerable<IdentityRole>? Roles { get; set; }
    }
}