

using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Application.Services.Authentication.Commands;

public class LoginCommand : CommandBase<CommandResult<AuthResponseDTO>>
{
    public LoginRequestDTO Model { get; set; }

    public LoginCommand(LoginRequestDTO model)
    {
        Model = model;
    }
    public class Handler : IRequestHandler<LoginCommand, CommandResult<AuthResponseDTO>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettings _settings;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _userService;
        public IConfiguration _configuration;

        public Handler(UserManager<AppUser> userManager, JwtSettings settings, IMediator mediator, ICurrentUserService userService, IConfiguration configuration)
        {
            _userManager = userManager;
            _settings = settings;
            _mediator = mediator;
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<CommandResult<AuthResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //if (!await GoogleRecaptchaHelper.IsReCaptchaPassedAsync(request.Model.Captcha, _configuration["GoogleRecaptcha:RecaptchaV3SecretKey"]))
            //    return CommandResult<AuthResponseDTO>.GetFailed("Captcha Hatası");


            AppUser user = await _userManager.FindByEmailAsync(request.Model.Email);
            if (user == null)
                return CommandResult<AuthResponseDTO>.GetFailed("E-mail ya da şifre hatalı !", new AuthResponseDTO());


            var result = await _userManager.CheckPasswordAsync(user, request.Model.Password);

            if (result)
            {
                // yetkiler belirlenmeli
                var userRoles = await _userManager.GetRolesAsync(user);
                var currentUser = new CurrentUser(user.Id, user.UserName, user.Email, userRoles.ToList()); // buraya kullanıcının teamid leri gelecek
                var claims = TokenModel.GetClaims(currentUser);
                var token = JwtHelper.CreateJwtToken(_settings, Audiences.Public, claims);
                user.LastLogin = DateTimeOffset.UtcNow;
                user.RefreshToken = JwtHelper.CreateRefreshToken(_settings, Audiences.Public, claims);
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpireDay);
                await _userManager.UpdateAsync(user);
                _userService.Authenticate(token);

                return CommandResult<AuthResponseDTO>.GetSucceed(new AuthResponseDTO(token, user.Email, user.RefreshToken, user.Id, userRoles.ToList()));

            }

            return CommandResult<AuthResponseDTO>.GetFailed("E-mail ya da şifre hatalı !", new AuthResponseDTO());
        }
    }
}
