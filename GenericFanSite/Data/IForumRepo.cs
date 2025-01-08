using GenericFanSite.Models;

namespace GenericFanSite.Data
{
    public interface IForumRepo
    {
        public List<ForumPost> GetAllForumPosts();  // Returns all forum post objects
        public ForumPost GetForumPostById(int id); // Returns a model object
        public int StoreForumPost(ForumPost model);  // Saves a model object to the db
    }
}
