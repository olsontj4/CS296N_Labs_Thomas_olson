using GenericFanSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GenericFanSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<AppUser> userMngr, RoleManager<IdentityRole> roleMngr)
        {
            _userManager = userMngr;
            _roleManager = roleMngr;
        }
        [HttpGet]
        public async Task<IActionResult> Index()  //Renders the admin page for managing users and roles
        {
            List<AppUser> appUsers = new List<AppUser>();
            foreach (AppUser appUser in _userManager.Users.ToList())
            {
                appUser.RoleNames = await _userManager.GetRolesAsync(appUser);
                appUsers.Add(appUser);
            }
            UserVM model = new UserVM
            {
                AppUsers = appUsers,
                Roles = _roleManager.Roles
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Add()  //Renders the form for adding users
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(RegisterVM model)  //Adds a user
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Username };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)  //Removes a user
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!(_userManager.IsInRoleAsync(user, "Admin").Result))  //Can't delete any admin user.
                {
                    IdentityResult result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    { // if failed
                        string errorMessage = "";
                        foreach (IdentityError error in result.Errors)
                        {
                            errorMessage += error.Description + " | ";
                        }
                        TempData["message"] = errorMessage;
                    }
                }
                else
                {
                    TempData["message"] = "Can't delete an admin user.";
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddToAdmin(string id)  //Adds a user to the "Admin" role
        {
            IdentityRole adminRole = await _roleManager.FindByNameAsync("Admin");
            if (adminRole == null)
            {
                TempData["message"] = "Admin role does not exist. "
                + "Click 'Create Admin Role' button to create it.";
            }
            else
            {
                AppUser user = await _userManager.FindByIdAsync(id);
                await _userManager.AddToRoleAsync(user, adminRole.Name);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromAdmin(string id)  //Removes a user from the "Admin" role
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdminRole()  //Creates the "Admin" role
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)  //Removes a role
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role.Name != "Admin")
            {
                await _roleManager.DeleteAsync(role);  //Can't delete admin role.
            }
            return RedirectToAction("Index");
        }
    }
}
