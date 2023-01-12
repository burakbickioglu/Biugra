namespace Biugra.Domain.Models;
public class UserProvidedService : BaseEntity
{
    public string AppUserId { get; set; }
    public virtual AppUser User { get; set; }
    public Guid ProvidedServiceId { get; set; }
    public virtual ProvidedService ProvidedService { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}
