﻿using GenericFanSite.Models;

namespace GenericFanSite.Data
{
    public class FakeForumRepo : IForumRepo
    {
        private List<ForumPost> forumPosts = new List<ForumPost>();
        public List<ForumPost> GetAllForumPosts()
        {
            return forumPosts;
        }
        public async Task<ForumPost> GetForumPostByIdAsync(int id)
        {
            ForumPost forumPost = forumPosts.Find(f => f.ForumPostId == id);
            return forumPost;
        }
        public async Task<int> StoreForumPostAsync(ForumPost model)
        {
            int status = 0;
            if (model != null)
            {
                model.Date = DateTime.Now;
                model.ForumPostId = forumPosts.Count + 1;
                forumPosts.Add(model);
                status = 1;
            }
            return status;
        }
        public async Task<int> UpdateForumPostAsync(ForumPost model)
        {
            return -1;
        }
        public int DeleteForumPost(int id)
        {
            /*context.Remove(id);
            return context.SaveChanges();*/
            throw new Exception();
        }
        public int DeleteComment(int forumPostId, int commentId)
        {
            throw new Exception();
        }
    }
}