namespace Biugra.Domain.Models.Dtos.ProvidedService;
public class UserProvidedServiceDTO
{
    public Guid Id { get; set; }
    public string AppUserId { get; set; }
    public virtual AppUser User { get; set; }
    public Guid ProvidedServiceId { get; set; }
    public virtual ProvidedServiceDTO ProvidedService { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
