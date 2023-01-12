
using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Services.Authentication.Commands;

public class GoogleLoginRequestCommand : CommandBase<CommandResult<AuthResponseDTO>>
{
    public ExternalAuthDTO Model { get; set; }

    public GoogleLoginRequestCommand(ExternalAuthDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<GoogleLoginRequestCommand, CommandResult<AuthResponseDTO>>
    {
        private readonly UserManager<AppUser> _userManager;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private readonly JwtSettings _settings;
        private readonly IGoogleLoginHelper _googleLoginHelper;
        public Handler(UserManager<AppUser> userManager, IMediator mediator, JwtSettings settings, IGoogleLoginHelper googleLoginHelper, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _mediator = mediator;
            _settings = settings;
            _googleLoginHelper = googleLoginHelper;
            _currentUserService = currentUserService;
        }

        public async Task<CommandResult<AuthResponseDTO>> Handle(GoogleLoginRequestCommand request, CancellationToken cancellationToken)
        {

            var payload = await _googleLoginHelper.VerifyGoogleToken(request.Model);
            if (payload == null)
                return CommandResult<AuthResponseDTO>.GetFailed(ErrorMessageConstants.INVALID_EXTERNAL_AUTH);
            //return BadRequest("Invalid External Authentication.");


            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new AppUser { Email = payload.Email, UserName = payload.Email };
                    await _userManager.CreateAsync(user);
                    //prepare and send an email for the email confirmation
                    await _userManager.AddToRoleAsync(user, Roles.SystemUser.ToString());
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }
            if (user == null)
                return CommandResult<AuthResponseDTO>.GetFailed(ErrorMessageConstants.INVALID_EXTERNAL_AUTH);
            //check for the Locked out account
            var userRoles = await _userManager.GetRolesAsync(user);
            var currentUser = new CurrentUser(user.Id, user.UserName, user.Email, userRoles.ToList()); // buraya kullanıcının teamid leri gelecek
            //var claims = TokenModel.GetClaims(currentUser);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, Roles.SystemUser.ToString()),
            };
            var token = JwtHelper.CreateJwtToken(_settings, Audiences.Public, claims);
            _currentUserService.Authenticate(token);
            return CommandResult<AuthResponseDTO>.GetSucceed(new AuthResponseDTO(token, user.Email, user.RefreshToken, user.Id, userRoles.ToList()));
        }
    }
}
