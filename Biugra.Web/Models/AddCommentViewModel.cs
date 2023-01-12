using Biugra.Domain.Models.Dtos.Comment;
using Biugra.Domain.Models.Dtos.Forum;

namespace Biugra.Web.Models;
public class AddCommentViewModel
{
    public AddCommentRequestDTO? NewComment{ get; set; }
    public ForumResponseDTO? Forum{ get; set; }

}
