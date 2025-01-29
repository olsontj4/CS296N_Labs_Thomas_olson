using Microsoft.AspNetCore.Identity;

namespace GenericFanSite.Models
{
    public class SeedUsers
    {
        public static async Task CreateAdminUserAsync(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            string username = "admin";
            string password = "Secret!123";
            string roleName1 = "Admin";
            string roleName2 = "Verified";
            if (await roleManager.FindByNameAsync(roleName1) == null)  // if role doesn't exist, create it
            {
                await roleManager.CreateAsync(new IdentityRole(roleName1));
            }
            if (await roleManager.FindByNameAsync(roleName2) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName2));
            }
            if (await userManager.FindByNameAsync(username) == null)  // if username doesn't exist, create it and add to role
            {
                AppUser appUser = new AppUser { UserName = username };
                var result = await userManager.CreateAsync(appUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, roleName1);
                }
            }
        }
    }
}