
using Biugra.Domain.Enums;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Interfaces;

public interface IMailService
{
    Task<string> SendMail(MailTypes mailtype, string subject, string body, List<string> toList, bool addccbcc = true);
    Task<CommandResult<string>> SendMailToUser(ContactMailViewModel mailModel);
}
