using Biugra.Domain.Enums;
using Biugra.Domain.Models;

namespace BaseProject.Domain.Models;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarImage { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    //public string? PhoneNumber { get; set; }
    public string? HighSchool { get; set; }
    public string? Address { get; set; }
    //public virtual ICollection<Forum>? Forums { get; set; }
    public virtual Vallet Vallet { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    //public AccountType AccountType { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastLogin { get; set; }
    //public virtual ICollection<string> UserTitles { get; set; }
    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    public virtual ICollection<UserProvidedService> UserProvidedServices { get; set; }
    //public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Message> Messages{ get; set; }
}
