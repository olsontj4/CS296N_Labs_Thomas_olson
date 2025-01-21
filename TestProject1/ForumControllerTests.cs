using GenericFanSite.Controllers;
using GenericFanSite.Data;
using GenericFanSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace FanSiteTests
{
    public class ForumControllerTests
    {
        private readonly ITestOutputHelper output;  // Console logging.
        IForumRepo repo = new FakeForumRepo();
        ForumController controller;
        private UserManager<AppUser> userMngr;

        public ForumControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            controller = new ForumController(repo, userMngr);
        }
        
        [Fact]
        public void ForumPostTestSuccess()
        {
            // arrange
            // Done in the constructor
            // act
            AppUser user1 = new AppUser();
            user1.UserName = "Test";
            ForumPost forumPost = new ForumPost();
            forumPost.User = user1;
            var result = controller.ForumPostForm(forumPost);
            // assert
            // Check to see if I got a RedirectToActionResult
            output.WriteLine(result.ToString());
            Assert.True(result.GetType() == typeof(RedirectToActionResult));
        }
        [Fact]
        public void ForumPostTestFailure()
        {
            // arrange
            // Done in the constructor
            // act
            var result = controller.ForumPostForm(null);
            // assert
            // Check to see if I got a RedirectToActionResult
            output.WriteLine(result.ToString());
            Assert.True(result.GetType() == typeof(ViewResult));
        }
        [Fact]
        public void ForumPostTestValidation()
        {
            AppUser user1 = new AppUser();
            user1.UserName = "Test";
            ForumPost forumPost = new ForumPost();
            forumPost.ForumPostId = 0;
            forumPost.Title = "Titleeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
            forumPost.User = user1;

            var result = controller.ForumPostForm(forumPost);
            output.WriteLine(result.ToString());
            Assert.True(result.GetType() == typeof(ViewResult));
        }
    }
}
