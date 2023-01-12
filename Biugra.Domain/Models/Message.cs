namespace Biugra.Domain.Models;
public class Message : BaseEntity
{
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public string Title { get; set; }
    public string Question { get; set; }
    public string? Answer { get; set; }
}
