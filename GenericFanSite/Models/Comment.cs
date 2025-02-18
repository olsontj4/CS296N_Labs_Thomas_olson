using System.ComponentModel.DataAnnotations;

namespace GenericFanSite.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required(ErrorMessage = "Comment cannot be empty.")]
        [StringLength(1000, ErrorMessage = "Max length of comment is 1000 characters.")]
        public string? CommentText { get; set; }
        [Required]
        public AppUser? User { get; set; }
        public DateTime Date { get; set; }
        public int ForumPostId { get; set; }
    }
}