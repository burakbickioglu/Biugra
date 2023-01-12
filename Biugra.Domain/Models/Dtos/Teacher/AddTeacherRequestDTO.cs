using Biugra.Domain.Enums;

namespace Biugra.Domain.Models.Dtos.Teacher;
public  class AddTeacherRequestDTO
{
    [Display(Name ="Ad Soyad")]
    public string Fullname { get; set; }
    public string? Image { get; set; }
    [Display(Name = "Yaş")]
    public int Age { get; set; }
    [Display(Name = "Cinsiyet")]
    public Gender Gender { get; set; }
}
