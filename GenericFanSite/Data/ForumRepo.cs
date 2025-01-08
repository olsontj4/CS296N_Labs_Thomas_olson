using GenericFanSite.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericFanSite.Data
{
    public class ForumRepo : IForumRepo
    {
        private AppDbContext context;
        public ForumRepo(AppDbContext appDbContext)
        {
            context = appDbContext;
        }
        List<ForumPost> IForumRepo.GetAllForumPosts()
        {
            var forumPosts = context.ForumPosts
                .Include(forumPost => forumPost.User)
                .ToList();
            return forumPosts;
        }
        ForumPost IForumRepo.GetForumPostById(int id)
        {
            var review = context.ForumPosts
                .Include(forumPost => forumPost.User) // returns AppUser object
                .Where(forumPost => forumPost.ForumPostId == id)
                .SingleOrDefault();
            return review;
        }
        int IForumRepo.StoreForumPost(ForumPost data)
        {
            data.Date = DateTime.Now;
            context.ForumPosts.Add(data);
            return context.SaveChanges();
            // returns a positive value if succussful
        }
    }
}
