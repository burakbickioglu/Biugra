namespace Biugra.Domain.Models.Dtos.ProvidedService;
public class AddProvidedServiceResponseDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
    public Guid? TeacherId { get; set; }
    public virtual Domain.Models.Teacher Teacher { get; set; }
}
