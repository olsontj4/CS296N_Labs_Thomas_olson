using System.ComponentModel.DataAnnotations;

namespace GenericFanSite.Models
{
    public class ForumPost
    {
        public int ForumPostId { get; set; }
        [Required(ErrorMessage = "Please enter a title.")]
        [StringLength(100, ErrorMessage = "Max length of title is 100 characters.")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Please add a description.")]
        [StringLength(200, ErrorMessage = "Max length of description is 200 characters.")]
        public string? Description { get; set;}
        [Required(ErrorMessage = "Please enter a year.")]
        [Range(1987, 9999, ErrorMessage = "Please enter a different year.")]
        public int? Year { get; set;}
        [Required(ErrorMessage = "Please write a story.")]
        [StringLength(1000, ErrorMessage = "Max length of story is 1000 characters.")]
        public string? Story { get; set;}
        [Required]
        public AppUser User { get; set;}
        public DateTime Date { get; set;}
    }
}
