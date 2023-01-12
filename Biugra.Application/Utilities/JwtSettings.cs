
namespace Biugra.Service.Utilities;

public class JwtSettings
{
    public string JwtSecret { get; set; }
    public string Issuer { get; set; }
    public uint TokenExpireDay { get; set; }
    public uint TokenExpireMinute { get; set; }
    public uint RefreshTokenExpireDay { get; set; }
}
