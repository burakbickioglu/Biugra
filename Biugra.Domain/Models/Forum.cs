using BaseProject.Domain.Models;

namespace Biugra.Domain.Models;
public class Forum : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Content { get; set; }
    public int LikeCount { get; set; }
    public bool IsHelpfull { get; set; }
    public string? Image { get; set; }
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public string AppUserId { get; set; }
    public List<Comment> Comments { get; set; }
}
