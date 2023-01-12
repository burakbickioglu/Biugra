namespace Biugra.Domain.Models;
public class ProvidedService : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
    public int? StudentCount { get; set; }
    public Guid? TeacherId { get; set; }
    public virtual Teacher Teacher { get; set; }
    public virtual ICollection<UserProvidedService> UserProvidedServices { get; set; }
}
