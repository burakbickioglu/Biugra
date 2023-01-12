
using Biugra.Domain.Models.Dtos;

namespace Biugra.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    protected readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("GetMe")]
    public async Task<CommandResult<AuthResponseDTO>> GetMe() => await _mediator.Send(new GetMeQuery());

    [HttpPost]
    [Route("Refresh")]
    public async Task<CommandResult<AuthResponseDTO>> Refresh([FromBody] RefreshRequestDTO model) => await _mediator.Send(new RefreshQuery(model));

    [HttpPost]
    [Route("Register")]
    public async Task<CommandResult<AuthResponseDTO>> Register([FromBody] RegisterRequestDTO model) => await _mediator.Send(new RegisterCommand(model));


    [HttpPost]
    [Route("Login")]
    public async Task<CommandResult<AuthResponseDTO>> Login([FromBody] LoginRequestDTO model) => await _mediator.Send(new LoginCommand(model));

    [HttpPost]
    [Route("GoogleLogin")]
    public async Task<CommandResult<AuthResponseDTO>> GoogleLogin([FromBody] ExternalAuthDTO model) => await _mediator.Send(new GoogleLoginRequestCommand(model));

    [HttpPost]
    [Route("ForgotPassword")]
    public async Task<CommandResult> ForgotPassword([FromBody] ForgotPasswordDTO model) => await _mediator.Send(new ForgotPasswordCommand(model));

    [HttpPost]
    [Route("ResetPassword")]
    public async Task<CommandResult> ResetPassword([FromBody] ResetPasswordDTO model) => await _mediator.Send(new ResetPasswordCommand(model));

    [HttpPost]
    [Route("ValidateResetPasswordToken")]
    public async Task<CommandResult> ValidateResetPasswordToken([FromBody] ForgotPasswordTokenValidationDTO token) => await _mediator.Send(new CheckForgotPasswordTokenValidateQuery(token));


}
