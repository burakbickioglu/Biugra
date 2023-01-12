using Biugra.Domain.Enums;

namespace Biugra.Domain.Models.Dtos.Teacher;
public class AddTeacherResponseDTO
{
    public Guid Id { get; set; }
    public string Fullname { get; set; }
    public string? Image { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
}
