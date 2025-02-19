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
        public List<ForumPost> GetAllForumPosts()
        {
            return context.ForumPosts
                .Include(forumPost => forumPost.User)
                .ToList();
        }
        public async Task<ForumPost> GetForumPostByIdAsync(int id)
        {
            return await context.ForumPosts
                .Include(forumPost => forumPost.User) // returns AppUser object
                .Where(forumPost => forumPost.ForumPostId == id)
                .SingleOrDefaultAsync();
        }
        public async Task<int> StoreForumPostAsync(ForumPost data)
        {
            data.Date = DateTime.Now;
            await context.ForumPosts.AddAsync(data);
            return context.SaveChanges();
            // returns a positive value if succussful
        }
        public int DeleteForumPost(int id)
        {
            ForumPost forumPost = GetForumPostByIdAsync(id).Result;
            context.ForumPosts.Remove(forumPost);
            return context.SaveChanges();
        }
    }
}
