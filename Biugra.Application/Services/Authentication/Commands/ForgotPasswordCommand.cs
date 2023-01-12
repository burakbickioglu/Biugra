using BaseProject.Domain.Models;
using Biugra.Domain.Enums;
using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Services.Authentication.Commands;

public class ForgotPasswordCommand : CommandBase<CommandResult>
{
    public ForgotPasswordDTO Model { get; set; }

    public ForgotPasswordCommand(ForgotPasswordDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<ForgotPasswordCommand, CommandResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettings _settings;
        public readonly IMailService _emailSender;
        private readonly IGeneralContentRepository<GeneralContent> _generalContentRepository;

        public Handler(UserManager<AppUser> userManager, JwtSettings settings, IMailService emailSender, IGeneralContentRepository<GeneralContent> generalContentRepository)
        {
            _userManager = userManager;
            _settings = settings;
            _emailSender = emailSender;
            _generalContentRepository = generalContentRepository;
        }

        public async Task<CommandResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Model.Email))
                return CommandResult.GetFailed(ErrorMessageConstants.EMPTY_MAIL);

            var user = await _userManager.FindByEmailAsync(request.Model.Email);
            //if(user == null)
            //{ // şifre gönderilemese bile mesaj şifre gönderilmiş gibi gönderilmeli, daha sonra istenebilir bundan dolayı yorum bıraktım
            //    return CommandResult.GetFailed("Şifre yenileme bağlantınız e-mail adresinize gönderildi.");
            //}

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var claims = new List<Claim>
            {
                new Claim("token", passwordResetToken),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var jwtToken = JwtHelper.CreateJwtToken(_settings, Audiences.Public, DateTime.UtcNow.AddDays(1), claims);
            var mail = await _generalContentRepository.GetFirst(p => p.Key == MailTypes.ResetPassword.ToString());
            var Key = request.Model.CallBackUrl + "?token=" + jwtToken;
            var mailContent = mail.Value.Replace("??Key??", Key).Replace("??UserName??", user.UserName);

            ContactMailViewModel contactMailViewModel = new ContactMailViewModel()
            {
                UserName = user.UserName,
                Content = mailContent,
                Email = user.Email,
                Subject = "Biugra Yeni Şifre Belirleme"
            };
            var emailResult = await _emailSender.SendMailToUser(contactMailViewModel);
            return CommandResult.GetSucceed();

        }
    }
}
