namespace GenericFanSite.Models
{
    public class ForumSearch
    {
        public String? Name { get; set; }
        public DateTime? Date { get; set; }
        public String? Filter { get; set; }
        public int Results { get; set; }
        public int NextPage { get; set; }
        public List<ForumPost>? ForumPosts { get; set; }
    }
}