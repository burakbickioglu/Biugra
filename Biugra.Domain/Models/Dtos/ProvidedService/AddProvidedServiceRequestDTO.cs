namespace Biugra.Domain.Models.Dtos.ProvidedService;
public class AddProvidedServiceRequestDTO
{
    [Display(Name = "Başlık")]
    public string Title { get; set; }
    [Display(Name = "Açıklama")]
    public string Description { get; set; }
    [Display(Name = "Ücret")]
    public decimal? Price { get; set; }
    [Display(Name = "Öğretmen")]
    public Guid? TeacherId { get; set; }
}
