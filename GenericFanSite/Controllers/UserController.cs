using GenericFanSite.Models;
using GenericFanSite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            foreach (AppUser appUser in _userManager.Users.OrderBy(a => a.SignUpDate).ToList())
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
                user.SignUpDate = DateTime.Now;
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
                    else
                    {
                        await _userManager.UpdateSecurityStampAsync(user);
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
        public async Task<IActionResult> AddToRole(string userId, string roleId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            AppUser user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddToRoleAsync(user, role.Name);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userId, string roleId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            AppUser user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRoleAsync(user, role.Name);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)  //Creates the "Admin" role
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)  //Removes a role
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role.Name != "Admin")  //Can't delete admin role.
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdatePassword(string id)
        {
            PasswordVM model = new()
            {
                id = id
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(PasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.id);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
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
    }
}
