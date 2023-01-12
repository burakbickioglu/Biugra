
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Services.Authentication.Queries;

public class CheckForgotPasswordTokenValidateQuery : CommandBase<CommandResult>
{
    public ForgotPasswordTokenValidationDTO Model { get; set; }

    public CheckForgotPasswordTokenValidateQuery(ForgotPasswordTokenValidationDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<CheckForgotPasswordTokenValidateQuery, CommandResult>
    {
        private readonly JwtSettings _settings;

        public Handler(JwtSettings settings)
        {
            _settings = settings;
        }

        public async Task<CommandResult> Handle(CheckForgotPasswordTokenValidateQuery request, CancellationToken cancellationToken)
        {
            var model = JwtHelper.GetPrincipalFromToken(_settings, request.Model.Token);
            var result = JwtHelper.IsExpired(request.Model.Token);
            if (result)
                return CommandResult.GetFailed(ErrorMessageConstants.INVALID_TOKEN);

            return CommandResult.GetSucceed();


        }
    }
}
