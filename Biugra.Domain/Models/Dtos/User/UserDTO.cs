namespace Biugra.Domain.Models.Dtos.User;

public class UserDTO
{
    public string Id { get; set; }
    [Display(Name = "Ad")]
    public string? FirstName { get; set; }
    [Display(Name = "Soyad")]
    public string? LastName { get; set; }
    [Display(Name = "E-mail")]
    public string? Email { get; set; }
    [Display(Name = "Yaş")]
    public int? Age { get; set; }
    [Display(Name = "Lise")]
    public string? HighSchool { get; set; }
    [Display(Name = "Adres")]
    public string? Address { get; set; }


}
