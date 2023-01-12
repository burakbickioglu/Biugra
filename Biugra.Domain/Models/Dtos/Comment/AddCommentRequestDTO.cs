namespace Biugra.Domain.Models.Dtos.Comment;
public class AddCommentRequestDTO
{
    public string? Title { get; set; }
    public string Description { get; set; }
    public Guid ForumId { get; set; }
}
