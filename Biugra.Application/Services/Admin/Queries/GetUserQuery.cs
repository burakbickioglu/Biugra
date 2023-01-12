using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.User;
using LanguageExt.Pipes;

namespace Biugra.Service.Services.Admin.Queries;

public class GetUserQuery : CommandBase<CommandResult<UserDTO>>
{
    public Guid UserId { get; set; }

    public GetUserQuery(Guid userId)
    {
        UserId = userId;
    }

    public class Handler : IRequestHandler<GetUserQuery, CommandResult<UserDTO>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public Handler(IMapper mapper, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<CommandResult<UserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return CommandResult<UserDTO>.GetFailed(ErrorMessageConstants.USER_NOT_FOUND);
            var response = _mapper.Map<UserDTO>(user);
            return CommandResult<UserDTO>.GetSucceed(response);
        }
    }
}