using GenericFanSite.Models;
using Microsoft.AspNetCore.Mvc;
using GenericFanSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;

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
        public async Task<IActionResult> Index(ForumSearchVM data)
        {
            int countFromResults = data.Results;
            if (data.Results == 0)  //Default for number of forum posts displayed is 5.
            {
                countFromResults = 5;
            }
            else if (data.Results == -1)  //Display all.
            {
                countFromResults = repo.GetAllForumPosts()
                    .ToList()
                    .Count;
            }
            if (data.Filter == "Name")
            {
                data.ForumPosts = repo.GetAllForumPosts()
                    .Where(p => data.Name == null || p.User.UserName == data.Name)
                    .Where(p => data.Date == null || p.Date == data.Date)
                    .OrderBy(p => p.User.UserName)
                    .Take(countFromResults)  //Using .Take() to not display every row in the database table.
                    .ToList();
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
            }
            return View(data);
        }
        [Authorize]
        public IActionResult ForumPostForm()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ForumPostForm(ForumPost data)
        {
            if (data == null)
            {
                return View();
            }
            if (userManager != null)  //Allowing null user only for unit testing.
            {
                data.User = await userManager.GetUserAsync(User);
            }
            ModelState.Remove(nameof(data.User));  //Ignoring user validation for now since you need to be logged in anyway to get here.
            if (ModelState.IsValid)
            {
                try
                {
                    if (data != null && await repo.StoreForumPostAsync(data) > 0)
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
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> ForumPostSingle(int id)
        {
            ForumPost data = repo.GetForumPostByIdAsync(id).Result;
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> ForumPostSingle(int id, Comment comment)
        {
            ForumPost data = repo.GetForumPostByIdAsync(id).Result;
            if (!(comment.CommentText == null))
            {
                comment.CommentText = comment.CommentText.Trim();
                comment.Date = DateTime.Now;
                comment.User = await userManager.GetUserAsync(User);
            }
            else
            {
                return View(data);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    data.Comments.Add(comment);
                    if (data != null && await repo.UpdateForumPostAsync(data) > 0)
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
            return View(data);
        }
    }
}