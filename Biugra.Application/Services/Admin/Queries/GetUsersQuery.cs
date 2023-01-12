using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.User;

namespace Biugra.Service.Services.Admin.Queries;

public class GetUsersQuery : CommandBase<CommandResult<List<UserDTO>>>
{
    public GetUsersQuery()
    {

    }

    public class Handler : IRequestHandler<GetUsersQuery, CommandResult<List<UserDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public Handler(IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CommandResult<List<UserDTO>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync();


            var response = _mapper.Map<List<UserDTO>>(users.ToList());
            return CommandResult<List<UserDTO>>.GetSucceed(response);
        }
    }
}
