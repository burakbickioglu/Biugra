
using BaseProject.Domain.Models;
using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Message;

namespace Biugra.Service.Mapper;

public class BaseEntityProfile : Profile
{
    public BaseEntityProfile()
    {
        CreateMap<BaseEntity, BaseEntityDTO>().ReverseMap();
        CreateMap<Message, MessageDTO>()
            .ForMember(src => src.AppUser, opt => opt.MapFrom(p => p.AppUser))
            .ReverseMap();
    }
}
