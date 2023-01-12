using BaseProject.Domain.Models;
using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.User;

namespace Biugra.Service.Services.Admin.Commands;

public class AddUserCommand : CommandBase<CommandResult>
{
    public AddUserDTO Model { get; set; }

    public AddUserCommand(AddUserDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<AddUserCommand, CommandResult>
    {
        private readonly UserManager<AppUser> _userManager;
        public readonly IMailService _emailSender;
        private readonly IGeneralContentRepository<GeneralContent> _generalContentRepository;

        public Handler(UserManager<AppUser> userManager, IMailService emailSender, IGeneralContentRepository<GeneralContent> generalContentRepository)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _generalContentRepository = generalContentRepository;
        }

        public async Task<CommandResult> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var User = await _userManager.FindByEmailAsync(request.Model.Email);
            if (User != null)
                return CommandResult.GetFailed(ErrorMessageConstants.INVALID_MAIL_OR_PASSWORD);

            AppUser createUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Model.Email,
                FirstName = request.Model.Name,
                LastName = request.Model.Surname,
                Email = request.Model.Email,
                //Country = request.Model.Country,
                //Title = request.Model.Title,
                AvatarImage = request.Model.AvatarImage,
                CreatedAt = DateTimeOffset.UtcNow
            };

            //var randomGenerator = new Random(123);
            //var password = randomGenerator.Next(153845, 999999).ToString();

            IdentityResult result = await _userManager.CreateAsync(createUser, request.Model.Password);
            var role = request.Model.IsAdmin ? Roles.Admin.ToString() : Roles.SystemUser.ToString();
            var roleResult = await _userManager.AddToRoleAsync(createUser, role);

            if (result.Succeeded)
            {
                //var mail = await _generalContentRepository.GetFirst(p => p.Key == MailTypes.WelcomeCustomer.ToString());
                //var mailContent = mail.Value.Replace("??UserName??", createUser.UserName);
                var mailContent = $"Hoşgeldin {createUser.Email}, geçici şifren : {request.Model.Password}";
                ContactMailViewModel contactMailViewModel = new ContactMailViewModel
                {
                    Email = createUser.Email,
                    Subject = "Hoş Geldiniz",
                    Content = mailContent,
                    UserName = createUser.UserName
                };
                var emailResult = await _emailSender.SendMailToUser(contactMailViewModel);
                return CommandResult.GetSucceed();
            }
            return CommandResult.GetFailed(ErrorMessageConstants.USER_CANT_CREATE);
        }


    }
}
