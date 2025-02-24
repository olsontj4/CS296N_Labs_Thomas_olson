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
        public async Task<ForumPost> GetForumPostByIdAsync(int forumPostId)
        {
            return await context.ForumPosts
                .Include(forumPost => forumPost.User) // returns AppUser object
                .Include(forumPost => forumPost.Comments)
                .ThenInclude(forumPost => forumPost.User)  //User is commenter, not Forum post author.
                .Where(forumPost => forumPost.ForumPostId == forumPostId)
                .SingleOrDefaultAsync();
        }
        public async Task<int> StoreForumPostAsync(ForumPost data)
        {
            data.Date = DateTime.Now;
            await context.ForumPosts.AddAsync(data);
            return context.SaveChanges();
            // returns a positive value if succussful
        }
        public async Task<int> UpdateForumPostAsync(ForumPost data)
        {
            context.ForumPosts.Update(data);
            return await context.SaveChangesAsync();
        }
        public int DeleteForumPost(int forumPostId)
        {
            ForumPost forumPost = GetForumPostByIdAsync(forumPostId).Result;
            context.ForumPosts.Remove(forumPost);
            return context.SaveChanges();
        }
        public int DeleteComment(int forumPostId, int commentId)
        {
            ForumPost forumPost = GetForumPostByIdAsync(forumPostId).Result;
            Comment comment = forumPost.Comments.Where(c => c.CommentId == commentId).SingleOrDefault();
            forumPost.Comments.Remove(comment);
            return context.SaveChanges();
        }
    }
}
