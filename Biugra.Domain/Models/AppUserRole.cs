
using BaseProject.Domain.Models;

namespace Biugra.Domain.Models;

public class AppUserRole : IdentityUserRole<string>
{
    public virtual AppUser User { get; set; }
    public virtual AppRole Role { get; set; }
}
