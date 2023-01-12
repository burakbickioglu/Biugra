
namespace Biugra.Domain.Models;
public class Category : BaseEntity
{
    public string Name { get; set; }
    public string Icon { get; set; }
    public List<Forum> Forums { get; set; }
}
