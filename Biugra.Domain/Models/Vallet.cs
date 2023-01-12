using System.ComponentModel.DataAnnotations.Schema;

namespace Biugra.Domain.Models;
public class Vallet : BaseEntity
{
    [ForeignKey("AppUser")]
    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
    public decimal Balance { get; set; }
}
