
using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Services.Authentication.Commands;

public class ResetPasswordCommand : CommandBase<CommandResult>
{
    public ResetPasswordDTO Model { get; set; }

    public ResetPasswordCommand(ResetPasswordDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<ResetPasswordCommand, CommandResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;
        private readonly JwtSettings _settings;
        public Handler(UserManager<AppUser> userManager, IMediator mediator, JwtSettings settings)
        {
            _userManager = userManager;
            _mediator = mediator;
            _settings = settings;
        }

        public async Task<CommandResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var model = JwtHelper.GetPrincipalFromToken(_settings, request.Model.Token);
            var email = model.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value ?? "";
            var token = model.Claims.FirstOrDefault(p => p.Type == "token")?.Value ?? "";

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return CommandResult.GetSucceed(ErrorMessageConstants.MAIL_SENDED);

            var webEncoders = WebEncoders.Base64UrlDecode(token);
            var encodingToken = Encoding.UTF8.GetString(webEncoders);

            var result = await _userManager.ResetPasswordAsync(user, encodingToken, request.Model.Password);


            if (result.Succeeded)
                return CommandResult.GetSucceed();

            return CommandResult.GetFailed(ErrorMessageConstants.INVALID_TOKEN);
        }
    }
}





























//using Microsoft.AspNetCore.WebUtilities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Biugra.Service.Services.Authentication.Commands
//{
//    public class ResetPasswordCommand : CommandBase<CommandResult<string>>
//    {
//        public ResetPasswordDTO Model { get; set; }

//        public ResetPasswordCommand(ResetPasswordDTO model)
//        {
//            Model = model;
//        }
//        public class Handler : IRequestHandler<ResetPasswordCommand, CommandResult<string>>
//        {
//            public readonly UserManager<AppUser> _userManager;

//            public Handler(UserManager<AppUser> userManager)
//            {
//                _userManager = userManager;
//            }

//            public async Task<CommandResult<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
//            {
//                //if (!await GoogleRecaptchaHelper.IsReCaptchaPassedAsync(captcha, captchaSecret))
//                //    return GenericApiResult<string>.GetFailed("Captcha Yanlış Girildi");

//                if (string.IsNullOrEmpty(request.Model.email))
//                    return CommandResult<string>.GetFailed("Email bilgisi gereklidir");

//                var user = await _userManager.FindByEmailAsync(request.Model.email);
//                //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
//                //{
//                //    var response =  await  ConfirmMailSender(user);
//                //    return BadRequest("Email Adresiniz Aktif Degildir Aktif Olması İçin Gelen Link İle Aktif Ediniz.");
//                //}

//                if (user == null)
//                    return CommandResult<string>.GetFailed("Kullanıcı bulunamadı, lütfen kaydolun");


//                var Token = await _userManager.GeneratePasswordResetTokenAsync(user);
//                var Encoding_ = Encoding.UTF8.GetBytes(Token);
//                var WebEncoders_ = WebEncoders.Base64UrlEncode(Encoding_);
//                ContactMailViewModel contactMailViewModel = new ContactMailViewModel()
//                {
//                    Key = request.Model.callbackUrl + "?userId=" + user.Id + "&token=" + WebEncoders_,
//                    UserName = user.UserName,
//                    ViewName = "MailTemplates/ResetPassword.cshtml",
//                    Email = user.Email,
//                    Subject = "Biugra Yeni Şifre Belirleme"
//                };
//                //return await _emailSender.SendMailToUser(contactMailViewModel);
//            }
//        }
//    }
//}
