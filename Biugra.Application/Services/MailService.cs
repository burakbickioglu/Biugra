

using Biugra.Domain.Enums;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Services;

public class MailService : IMailService
{
    public IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> SendMail(MailTypes mailtype, string subject, string body, List<string> toList, bool addccbcc = true)
    {
        string result = "";
        try
        {
            var ccList = new List<string>();
            var bccList = new List<string>();
            var client = new SendGridClient(_configuration["MailSettings:SENDGRIDAPIKEY"]);
            var sender = _configuration["MailSettings:SENDGRIDSender"];


            var msg = new SendGridMessage()
            {
                From = new EmailAddress(sender, "Biugra"),
                Subject = subject,
                HtmlContent = body,
            };

            if (toList != null)
            {
                foreach (var item in toList)
                {
                    msg.AddTo(new EmailAddress(item));
                }
            }

            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            if (IsSendGridResponseOK(response.StatusCode) == false)
            {
                result = await response.Body.ReadAsStringAsync();
            }
        }
        catch (Exception x)
        {
            result = x.Message;
        }
        return result;
    }

    public bool IsSendGridResponseOK(System.Net.HttpStatusCode statusCode)
    {
        if (statusCode != System.Net.HttpStatusCode.OK && statusCode != System.Net.HttpStatusCode.Accepted && statusCode != System.Net.HttpStatusCode.Created && statusCode != System.Net.HttpStatusCode.NonAuthoritativeInformation
            && statusCode != System.Net.HttpStatusCode.NoContent && statusCode != System.Net.HttpStatusCode.ResetContent && statusCode != System.Net.HttpStatusCode.PartialContent)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public async Task<CommandResult<string>> SendMailToUser(ContactMailViewModel mailModel)
    {
        if (!string.IsNullOrEmpty(mailModel.Email))
        {
            var tolist = new List<string>() { mailModel.Email };
            var mailresult = await SendMail(mailModel.MailTypes, mailModel.Subject, mailModel.Content, tolist);
            return CommandResult<string>.GetSucceed(mailresult);
        }
        return CommandResult<string>.GetFailed("Email boş");
    }
}
