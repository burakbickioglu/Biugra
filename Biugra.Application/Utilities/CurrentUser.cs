

namespace Biugra.Service.Utilities;

public class CurrentUser
{
    public CurrentUser(string userId, string userName, string userMail, List<string> role)
    {
        UserId = userId;
        UserName = userName;
        UserMail = userMail;
        Role = role;

    }

    public string UserId { get; set; }
    public string UserName { get; set; }
    public string UserMail { get; set; }
    public List<string> Role { get; set; }
    //public List<Guid>? TeamIds { get; set; }

}
