namespace Biugra.Domain.Models.Dtos.Comment;
public class AddCommentResponseDTO
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; }
    public Guid ForumId { get; set; }
    public string AppUserId { get; set; }
}
