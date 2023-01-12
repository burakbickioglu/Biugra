namespace Biugra.Domain.Models.Dtos.ProvidedService;
public class ProvidedServiceDTO
{
    public Guid Id { get; set; }
    [Display(Name ="Başlık")]
    public string Title { get; set; }
    [Display(Name = "Açıklama")]
    public string Description { get; set; }
    [Display(Name = "Öğrenci Sayısı")]
    public int? StudentCount { get; set; }

    [Display(Name = "Ücret")]
    public decimal? Price { get; set; }
    [Display(Name = "Öğretmen")]
    public Guid? TeacherId { get; set; }
    public virtual Domain.Models.Teacher? Teacher { get; set; }
}
