using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Comment;
using Biugra.Domain.Models.Dtos.User;

namespace Biugra.Domain.Models.Dtos.Forum;
public class ForumResponseDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public int LikeCount { get; set; }
    public bool IsHelpfull { get; set; }
    public string? Image { get; set; }
    public Guid CategoryId { get; set; }
    public virtual CategoryDTO Category{ get; set; }
    public string AppUserId { get; set; }
    public string UserName { get; set; }
    public string UserLastName { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public List<CommentResponseDTO> Comments { get; set; }
}
