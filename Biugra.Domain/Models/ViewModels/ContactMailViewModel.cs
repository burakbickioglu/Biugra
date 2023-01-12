

using Biugra.Domain.Enums;

namespace Biugra.Domain.Models.ViewModels
{
    public class ContactMailViewModel
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Password { get; set; }
        public MailTypes MailTypes { get; set; }
    }
}
