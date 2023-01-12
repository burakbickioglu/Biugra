
namespace Biugra.Domain.Models;
public class Comment : BaseEntity
{
    public string? Title { get; set; }
    public string Description { get; set; }
    public Guid ForumId { get; set; }
    public virtual Forum Forum { get; set; }
    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
}
