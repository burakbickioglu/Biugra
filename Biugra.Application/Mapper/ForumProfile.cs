using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos.User;

namespace Biugra.Service.Mapper;
public class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<Forum, AddForumRequestDTO>().ReverseMap();
        CreateMap<Forum, AddForumResponseDTO>().ReverseMap();
        CreateMap<Forum, ForumResponseDTO>()
            .ForMember(src => src.Comments, opt => opt.MapFrom(x => x.Comments))
            .ForMember(src => src.Category, opt => opt.MapFrom(x => x.Category))
            .AfterMap<SetCommentUsersAction>();
    }

    public class SetCommentUsersAction : IMappingAction<Forum, ForumResponseDTO>
    {

        public SetCommentUsersAction()
        {
        }
        public void Process(Forum source, ForumResponseDTO destination, ResolutionContext context)
        {
            foreach (var item in destination.Comments)
            {
                var currentComment = source.Comments.FirstOrDefault(p => p.Id == item.Id);
                item.User = new UserDTO
                {
                    Id = currentComment.AppUser.Id,
                    Email = currentComment.AppUser.Email,
                    FirstName = currentComment.AppUser.FirstName,
                    LastName = currentComment.AppUser.LastName
                };
            }
        }

    }

}
