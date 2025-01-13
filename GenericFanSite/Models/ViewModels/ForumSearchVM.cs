namespace GenericFanSite.Models
{
    public class ForumSearchVM
    {
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public string? Filter { get; set; }
        public int Results { get; set; }
        public int NextPage { get; set; }
        public List<ForumPost>? ForumPosts { get; set; }
    }
}