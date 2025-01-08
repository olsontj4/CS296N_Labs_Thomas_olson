using GenericFanSite.Models;

namespace GenericFanSite.Data
{
    public class FakeForumRepo : IForumRepo
    {
        private List<ForumPost> forumPosts = new List<ForumPost>();
        List<ForumPost> IForumRepo.GetAllForumPosts()
        {
            return forumPosts;
        }
        public ForumPost GetForumPostById(int id)
        {
            ForumPost forumPost = forumPosts.Find(f => f.ForumPostId == id);
            return forumPost;
        }
        int IForumRepo.StoreForumPost(ForumPost model)
        {
            int status = 0;
            if (model != null && model.User != null)
            {
                model.Date = DateTime.Now;
                model.ForumPostId = forumPosts.Count + 1;
                forumPosts.Add(model);
                status = 1;
            }
            return status;
        }
    }
}