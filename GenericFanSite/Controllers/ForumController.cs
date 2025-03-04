using GenericFanSite.Models;
using Microsoft.AspNetCore.Mvc;
using GenericFanSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GenericFanSite.Models.ViewModels;

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
            if (data.Year > DateTime.Now.Year)
            {
                ModelState.AddModelError("Year", "Year can't be greater than " + DateTime.Now.Year.ToString() + ".");
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
                        ModelState.AddModelError(string.Empty, "There was a really bad error saving the forum post.");
                        return View();
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "An unknown error has occured.");
                    return View(data);
                }
            }
            return View(data);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ForumPostSingle(int forumPostId)
        {
            var model = new ForumPostSingleVM();
            model.ForumPost = await repo.GetForumPostByIdAsync(forumPostId);
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ForumPostSingle(ForumPostSingleVM data)
        {
            var model = new ForumPostSingleVM();
            model.ForumPost = await repo.GetForumPostByIdAsync(data.ForumPost.ForumPostId);
            if (data.NewComment?.CommentText != null)
            {
                data.NewComment.CommentText = data.NewComment.CommentText.Trim();
                data.NewComment.Date = DateTime.Now;
                data.NewComment.User = await userManager.GetUserAsync(User);
            }
            else
            {
                return View(model);
            }
            ModelState.Remove("ForumPost.Title");
            ModelState.Remove("ForumPost.Description");
            ModelState.Remove("ForumPost.Story");
            ModelState.Remove("ForumPost.Year");
            ModelState.Remove("ForumPost.Date");
            ModelState.Remove("ForumPost.User");
            ModelState.Remove("NewComment.User");
            model.NewComment = data.NewComment;
            if (ModelState.IsValid)
            {
                try
                {
                    model.ForumPost.Comments.Add(data.NewComment);
                    if (model.ForumPost != null)
                    {
                        await repo.UpdateForumPostAsync(model.ForumPost);
                        model.ForumPost = await repo.GetForumPostByIdAsync(data.ForumPost.ForumPostId);
                        model.NewComment = null;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "There was a really bad error saving the forum post.");
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "An unknown error has occured.");
                }
            }
            return View(model);
        }
        [Authorize]
        public IActionResult DeleteForumPost(int forumPostId)
        {
            if (repo.DeleteForumPost(forumPostId) > 0) {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("ForumPostSingle", forumPostId);
            }
        }
        [Authorize]
        public IActionResult DeleteComment(int forumPostId, int commentId)
        {
            repo.DeleteComment(forumPostId, commentId);
            return RedirectToAction("ForumPostSingle", "Forum", new { forumPostId = forumPostId });
        }
    }
}