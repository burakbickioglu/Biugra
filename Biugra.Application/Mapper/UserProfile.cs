using BaseProject.Domain.Models;
using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos.Notification;
using Biugra.Domain.Models.Dtos.User;
using Biugra.Domain.Models.Dtos.Wallet;

namespace Biugra.Service.Mapper;
public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<AppUser, UserDTO>().ReverseMap();
		CreateMap<Vallet, WalletDTO>().ReverseMap();
		CreateMap<Notification, NotificationDTO>().ReverseMap();
	}
}
