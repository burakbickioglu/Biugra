using Biugra.Domain.Models.Dtos.User;

namespace Biugra.Domain.Models.Dtos.Comment;
public class CommentResponseDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; }
    public Guid ForumId { get; set; }
    public string AppUserId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public UserDTO User { get; set; }
}
