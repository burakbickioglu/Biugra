using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Forum;

namespace Biugra.Web.Models;
public class AddForumViewModel
{
    public AddForumRequestDTO? Forum{ get; set; }
    public List<CategoryDTO>? Categories{ get; set; }
}
