using GenericFanSite.Models;

namespace GenericFanSite.Data
{
    public class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.ForumPosts.Any())  // this is to prevent adding duplicate data
            {
                // Create User objects
                AppUser user1 = new AppUser { Name = "Thomasj041" };
                AppUser user2 = new AppUser { Name = "Than" };
                AppUser user3 = new AppUser { Name = "Helena" };
                AppUser user4 = new AppUser { Name = "Brooke" };
                // Queue up user objects to be saved to the DB
                context.AppUsers.Add(user1);
                context.AppUsers.Add(user2);
                context.AppUsers.Add(user3);
                context.AppUsers.Add(user4);
                context.SaveChanges();  // Saving adds UserId to User objects
                ForumPost forumPost1 = new ForumPost
                {
                    Title = "Cool new album!",
                    Description = "Review of the Paradise State of Mind album",
                    Year = 2024,
                    Story = "This album sounds pretty good.",
                    User = user1,
                    Date = DateTime.Parse("11/25/2024")
                };
                ForumPost forumPost2 = new ForumPost
                {
                    Title = "Man, this vinyl is great.",
                    Description = "I bought the Supermodel album on vinyl.",
                    Year = 2024,
                    Story = "I bought the vinyl not long ago, and it's become one of my favourite albums ever! It was already a great album, but it sounds so crisp on my record player! Definitely one of the best purchases I've made.",
                    User = user2,
                    Date = DateTime.Parse("11/26/2024")
                };
                ForumPost forumPost3 = new ForumPost
                {
                    Title = "I love It's OK To Be Human",
                    Description = "Their best song, musically and otherwise.",
                    Year = 1987,
                    Story = "Truly an emotional rollercoaster, one of the best experiences I've been through. The story it immerses you into is immaculate, telling many different and yet complete and introspective narratives. It tells so much about human life and about life itself, so many parallels and allegories, about the duality of existence itself. And they don't dumb it down for their audience, they're one of the few bands that respect their audience. They know we'll understand the beauty they unfold through song form. When he sang \"Backwards and forwards\" I cried. This song has always been my escape from depressive situations. It helped me gain confidence, to truly feel and experience the world around me, and to understand. And the chords and melodies work together like no other song, nothing even comes close! I doubt there's many that would, or even 𝑐𝑜𝑢𝑙𝑑 dislike this song. Changed my life when I first heard it, and I don't know where I'd be without it. Thanks Lorem Ipsum, from a truly grateful fan.",
                    User = user3,
                    Date = DateTime.Parse("11/27/2024")
                };
                ForumPost forumPost4 = new ForumPost
                {
                    Title = "Supermodel is a good album",
                    Description = "these are words",
                    Year = 1999,
                    Story = "Never really listened much to Foster the People (aside from 'Pumped up Kicks') before listening to Supermodel. I find some of the lyrics pretty thought provoking, for example, one of my favorite songs from the album, 'Ask Yourself' goes like this: Well, I've tried to live like the way that you wanted me to Never gonna give you up, Never gonna let you down, never gonna run around or desert you",
                    User = user4,
                    Date = DateTime.Parse("11/27/2024")
                };
                context.ForumPosts.Add(forumPost1);  // queues up a review to be added to the DB
                context.ForumPosts.Add(forumPost2);
                context.ForumPosts.Add(forumPost3);
                context.ForumPosts.Add(forumPost4);
                context.SaveChanges(); // stores all the reviews in the DB
            }
        }
    }
}