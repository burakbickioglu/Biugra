using Biugra.Domain.Enums;

namespace Biugra.Domain.Models;
public class Teacher : BaseEntity
{
    public string Fullname { get; set; }
    public string? Image { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
}
