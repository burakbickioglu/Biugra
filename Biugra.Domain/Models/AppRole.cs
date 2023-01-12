
namespace BaseProject.Domain.Models;

public class AppRole : IdentityRole
{
    public virtual ICollection<AppUserRole> UserRoles { get; set; }

}
