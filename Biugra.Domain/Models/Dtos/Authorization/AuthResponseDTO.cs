
namespace Biugra.Domain.Models.Dtos.Authorization;

public class AuthResponseDTO
{
    public AuthResponseDTO(string? token, string? email, string? refreshtoken, string? userId, List<string> roles)
    {
        Token = token;
        Email = email;
        RefreshToken = refreshtoken;
        UserId = userId;
        Roles = roles;
    }
    public AuthResponseDTO(string? token, string? email, string? userId)
    {
        Token = token;
        Email = email;
        UserId = userId;
    }

    public AuthResponseDTO()
    {

    }
    public string? Token { get; set; }
    public string? Email { get; set; }
    //public string? UserRole { get; set; }
    public string? UserId { get; set; }
    public string? RefreshToken { get; set; }
    public List<string> Roles { get; set; }

}
