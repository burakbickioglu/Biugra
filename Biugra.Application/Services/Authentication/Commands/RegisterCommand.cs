
using BaseProject.Domain.Models;
using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Infrastructure.Interfaces.Repositories;
using System.Linq.Expressions;

namespace Biugra.Application.Services.Authentication.Commands;

public class RegisterCommand : CommandBase<CommandResult<AuthResponseDTO>>
{
    public RegisterRequestDTO Model { get; set; }

    public RegisterCommand(RegisterRequestDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<RegisterCommand, CommandResult<AuthResponseDTO>>
    {
        private readonly UserManager<AppUser> _userManager;
        protected readonly IMediator _mediator;
        public readonly IMailService _emailSender;
        public IConfiguration _configuration;
        private readonly IGeneralContentRepository<GeneralContent> _generalContentRepository;
        private readonly IGenericRepository<Vallet> _valletRepository;
        public Handler(UserManager<AppUser> userManager, IMediator mediator, IMailService emailSender, IConfiguration configuration, IGeneralContentRepository<GeneralContent> generalContentRepository, IGenericRepository<Vallet> valletRepository)
        {
            _userManager = userManager;
            _mediator = mediator;
            _emailSender = emailSender;
            _configuration = configuration;
            _generalContentRepository = generalContentRepository;
            _valletRepository = valletRepository;
        }

        public async Task<CommandResult<AuthResponseDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {


            var User = await _userManager.FindByEmailAsync(request.Model.Email);
            if (User != null)
                return CommandResult<AuthResponseDTO>.GetFailed(ErrorMessageConstants.INVALID_MAIL_OR_PASSWORD);

            AppUser createUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Model.Email,
                Email = request.Model.Email,
                CreatedAt = DateTimeOffset.UtcNow
            };
            IdentityResult result = await _userManager.CreateAsync(createUser, request.Model.Password);
            var roleResult = await _userManager.AddToRoleAsync(createUser, Roles.SystemUser.ToString());
            var valletResult = await _valletRepository.Add(new Vallet { AppUser = createUser, Balance = 0 });
            if (result.Succeeded)
            {
                return await _mediator.Send(new LoginCommand(new LoginRequestDTO(createUser.Email, request.Model.Password)));
            }

            var errorMessages = "";
            foreach (var error in result.Errors)
            {
                errorMessages += $"{error.Code} - {error.Description}";
            }
            return CommandResult<AuthResponseDTO>.GetFailed(errorMessages);
        }
    }
}
