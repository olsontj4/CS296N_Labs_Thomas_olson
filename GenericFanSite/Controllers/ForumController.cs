using GenericFanSite.Models;
using Microsoft.AspNetCore.Mvc;
using GenericFanSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GenericFanSite.Controllers
{
    public class ForumController : Controller
    {
        private IForumRepo repo;
        private UserManager<AppUser> userManager;
        public ForumController(IForumRepo r, UserManager<AppUser> userMngr)
        {
            userManager = userMngr;
            repo = r;
        }
        [HttpGet]
        public IActionResult Index(ForumSearchVM data)
        {
            int countFromResults = data.Results;
            if (data.Results == 0)  //Default for number of forum posts displayed is 5.
            {
                countFromResults = 5;
            }
            else if (data.Results == -1)  //Display all.
            {
                countFromResults = repo.GetAllForumPosts().ToList().Count;
            }
            if (data.Filter == "Name")
            {
                var forumPosts = repo.GetAllForumPosts()
                    .Where(p => data.Name == null || p.User.UserName == data.Name)
                    .Where(p => data.Date == null || p.Date == data.Date)
                    .OrderBy(p => p.User.UserName)
                    .Take(countFromResults)  //Using .Take() to not display every row in the database table.
                    .ToList();
                data.ForumPosts = forumPosts;
                return View(data);
            }
            else if (data.Filter == "Date (Oldest)")
            {
                var forumPosts = repo.GetAllForumPosts()
                    .Where(p => data.Name == null || p.User.UserName == data.Name)
                    .Where(p => data.Date == null || p.Date == data.Date)
                    .OrderBy(p => p.Date)
                    .Take(countFromResults)
                    .ToList();
                data.ForumPosts = forumPosts;
                return View(data);
            }
            else
            {
                var forumPosts = repo.GetAllForumPosts()
                    .Where(p => data.Name == null || p.User.UserName == data.Name)
                    .Where(p => data.Date == null || p.Date == data.Date)
                    .OrderByDescending(p => p.Date)
                    .Take(countFromResults)
                    .ToList();
                data.ForumPosts = forumPosts;
                return View(data);
            }
        }
        [Authorize]
        public IActionResult ForumPostForm()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult ForumPostForm(ForumPost data)
        {
            data.User = userManager.GetUserAsync(User).Result;
            ModelState.Remove(nameof(data.User));
            if (ModelState.IsValid)
            {
                try
                {
                    if (data != null && repo.StoreForumPost(data) > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.RedText = "There was a really bad error saving the forum post.";
                        return View();
                    }
                }
                catch
                {
                    ViewBag.RedText = "An unknown error has occured.";
                    return View(data);
                }
            }
            var allErrors = ModelState.Where(e => e.Value.Errors.Count > 0).ToList();
            ViewBag.RedText = allErrors[0].Value.Errors[0].ErrorMessage.ToString();  //I am doing a bad.  Error messages work though, so I don't care.  Thank you, Stack Overflow.
            return View(data);
        }
    }
}