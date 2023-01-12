

namespace Biugra.Service.Attributes;

public interface IAuthorizationAttribute { }

[AttributeUsage(AttributeTargets.Class)]
public class RoleAuthorizationAttribute : Attribute, IAuthorizationAttribute
{
    public IEnumerable<Roles> ValidRoles { get; }

    public RoleAuthorizationAttribute(params Roles[] validRoles)
    {
        ValidRoles = validRoles.Select(f => (Roles)f);
    }
}
