
namespace Biugra.Domain.Models.Dtos.Authorization;

public class LoginRequestDTO
{
    [Required]
    [Display(Name = "E-mail")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Şifre")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public LoginRequestDTO()
    {

    }

    public LoginRequestDTO(string email, string password)
    {
        Email = email;
        Password = password;
    }


}
