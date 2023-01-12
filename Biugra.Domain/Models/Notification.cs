namespace Biugra.Domain.Models;
public class Notification : BaseEntity
{
    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
    public string Message { get; set; }
}
