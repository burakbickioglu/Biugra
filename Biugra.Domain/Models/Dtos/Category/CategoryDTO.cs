using System.ComponentModel;

namespace Biugra.Domain.Models.Dtos.Category;
public class CategoryDTO
{
    public Guid Id { get; set; }
    [DisplayName("Kategori Adı")]
    public string Name { get; set; }
    public string Icon { get; set; }

}
