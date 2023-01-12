namespace Biugra.Domain.Models.Dtos.Forum;
public class AddForumRequestDTO
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Content { get; set; }
    public string? Image { get; set; }
    public Guid CategoryId { get; set; }
}
