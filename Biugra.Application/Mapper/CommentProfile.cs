using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos.Comment;

namespace Biugra.Service.Mapper;
public class CommentProfile : Profile
{
	public CommentProfile()
	{
		CreateMap<Comment, CommentResponseDTO>().ReverseMap();
		CreateMap<Comment, AddCommentResponseDTO>().ReverseMap();
		CreateMap<Comment, AddCommentRequestDTO>().ReverseMap();
	}
}
