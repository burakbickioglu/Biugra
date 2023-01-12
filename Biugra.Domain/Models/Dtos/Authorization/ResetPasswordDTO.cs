
namespace Biugra.Domain.Models.Dtos.Authorization;

public class ResetPasswordDTO
{
    //public string Email { get; set; }
    public string Token { get; set; }
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}
