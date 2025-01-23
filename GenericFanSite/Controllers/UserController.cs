using GenericFanSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GenericFanSite.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
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
            return RedirectToAction("Register", "Account");
        }
        [HttpPost]
        public async Task<IActionResult> Add(RegisterVM model)  //Adds a user
        {
            if (ModelState.IsValid)
            {
                var appUser = new AppUser { UserName = model.Username };
                var result = await _userManager.CreateAsync(appUser, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(appUser, isPersistent: false);
                    return RedirectToAction("ForumPostForm", "Forum");
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
        [HttpDelete]
        public async Task<IActionResult> Delete(string userName)  //Removes a user
        {
            return View("Index");
        }
        [HttpPut]
        public async Task<IActionResult> AddToAdmin(string id)  //Adds a user to the "Admin" role
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                throw new Exception();
            }
        }
        [HttpPut]
        public IActionResult RemoveFromAdmin(string userName)  //Removes a user from the "Admin" role
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdminRole()  //Creates the "Admin" role
        {
            var result = await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                throw new Exception();
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string id)  //Removes a role
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
