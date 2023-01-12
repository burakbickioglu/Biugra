
namespace Biugra.Domain.Models.Dtos.Authorization;

public class ForgotPasswordDTO
{
    [Required]
    [Display(Name = "E-mail")]
    public string Email { get; set; }
    public string CallBackUrl { get; set; }
}


public class ForgotPasswordResponseDTO
{
    public ForgotPasswordResponseDTO(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public bool Success { get; set; }
    public string Message { get; set; }
}
