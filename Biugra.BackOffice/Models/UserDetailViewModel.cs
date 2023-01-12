using Biugra.Domain.Models.Dtos.Forum;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.User;
using Biugra.Domain.Models.Dtos.Wallet;

namespace Biugra.BackOffice.Models;
public class UserDetailViewModel
{
    public UserDTO? User { get; set; }
    public List<ForumResponseDTO>? Forums { get; set; }
    public List<UserProvidedServiceDTO>? ProvidedServices { get; set; }
    public WalletDTO? Wallet { get; set; }
}
