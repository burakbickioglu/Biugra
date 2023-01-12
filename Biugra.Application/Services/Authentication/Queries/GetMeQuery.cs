
using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Services.Authentication.Queries;

public class GetMeQuery : CommandBase<CommandResult<AuthResponseDTO>>
{


    public class Handler : IRequestHandler<GetMeQuery, CommandResult<AuthResponseDTO>>
    {
        private readonly ICurrentUserService _userService;
        private readonly JwtSettings _settings;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;
        public Handler(ICurrentUserService userService, JwtSettings jwtSettings, UserManager<AppUser> userManager, IMediator mediator)
        {
            _userService = userService;
            _settings = jwtSettings;
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<CommandResult<AuthResponseDTO>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var principals = JwtHelper.GetPrincipalFromToken(_settings, _userService.GetToken());

            if (principals == null || principals.Identity == null)
                return CommandResult<AuthResponseDTO>.NotFound();

            var email = _userService.GetUserEmail();
            var user = await _userManager.FindByEmailAsync(email);
            var userRoles = await _userManager.GetRolesAsync(user);

            return CommandResult<AuthResponseDTO>.GetSucceed(new AuthResponseDTO(_userService.GetToken(), user.Email, user.RefreshToken, user.Id, userRoles.ToList()));
        }
    }
}
