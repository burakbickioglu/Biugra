using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos.ProvidedService;

namespace Biugra.Service.Mapper;
public class ProvidedServiceProfile : Profile
{
	public ProvidedServiceProfile()
	{
		CreateMap<ProvidedService, ProvidedServiceDTO>().ReverseMap();
		CreateMap<ProvidedService, AddProvidedServiceRequestDTO>().ReverseMap();
		CreateMap<ProvidedService, AddProvidedServiceResponseDTO>().ReverseMap();

		CreateMap<UserProvidedService, UserProvidedServiceDTO>()
			.ForMember(src=>src.ProvidedService, opt=>opt.MapFrom(p=>p.ProvidedService))
			.ReverseMap();
	}
}
