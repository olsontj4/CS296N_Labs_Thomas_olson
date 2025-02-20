using GenericFanSite.Models;

namespace GenericFanSite.Data
{
    public interface IForumRepo
    {
        public List<ForumPost> GetAllForumPosts();  // Returns all forum post objects
        public Task<ForumPost> GetForumPostByIdAsync(int id); // Returns a model object
        public Task<int> StoreForumPostAsync(ForumPost model);  // Saves a model object to the db
        public Task<int> UpdateForumPostAsync(ForumPost model);  //For adding comments to forum posts.
        public int DeleteForumPost(int id);  // Deletes a model object from the db
    }
}