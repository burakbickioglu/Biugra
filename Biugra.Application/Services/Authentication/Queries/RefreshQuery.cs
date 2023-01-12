

using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Services.Authentication.Queries;

public class RefreshQuery : CommandBase<CommandResult<AuthResponseDTO>>
{
    public RefreshRequestDTO Model { get; set; }

    public RefreshQuery(RefreshRequestDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<RefreshQuery, CommandResult<AuthResponseDTO>>
    {
        private readonly ICurrentUserService _userService;
        private readonly JwtSettings _settings;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;
        public Handler(ICurrentUserService userService, JwtSettings settings, UserManager<AppUser> userManager, IMediator mediator)
        {
            _userService = userService;
            _settings = settings;
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<CommandResult<AuthResponseDTO>> Handle(RefreshQuery request, CancellationToken cancellationToken)
        {
            var User = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == request.Model.RefreshToken);
            if (User == null)
                return CommandResult<AuthResponseDTO>.NotFound();
            if (User.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return CommandResult<AuthResponseDTO>.GetFailed(ErrorMessageConstants.INVALID_TOKEN);

            var userRoles = await _userManager.GetRolesAsync(User);
            var currentUser = new CurrentUser(User.Id, User.UserName, User.Email, userRoles.ToList());
            var claims = TokenModel.GetClaims(currentUser);
            //var auth = authClaims;
            var token = JwtHelper.CreateJwtToken(_settings, Audiences.Public, claims);
            _userService.Authenticate(token);
            User.RefreshToken = JwtHelper.CreateRefreshToken(_settings, Audiences.Public, claims);
            User.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpireDay);
            await _userManager.UpdateAsync(User);
            return CommandResult<AuthResponseDTO>.GetSucceed(new AuthResponseDTO(token, User.Email, User.RefreshToken, User.Id, userRoles.ToList()));

        }
    }
}
