
namespace Biugra.Service.Utilities.Helpers;

public static class JwtHelper
{
    internal static string CreateJwtToken(JwtSettings settings, Audiences audience, DateTime expires,
                                        IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(settings.JwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = settings.Issuer,
            Audience = audience.ToString(),
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    internal static string CreateRefreshToken(JwtSettings settings, Audiences audience, DateTime expires,
                                        IEnumerable<Claim> claims)
    {

        return GenerateRefreshToken();
    }

    internal static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
        rngCryptoServiceProvider.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    public static string CreateJwtToken(JwtSettings settings, Audiences audience,
                                        IEnumerable<Claim> claims)
    {
        return CreateJwtToken(settings, audience, DateTime.UtcNow.AddMinutes(settings.TokenExpireMinute), claims);
    }

    public static string CreateRefreshToken(JwtSettings settings, Audiences audience,
                                        IEnumerable<Claim> claims)
    {
        return CreateRefreshToken(settings, audience, DateTime.UtcNow.AddDays(settings.RefreshTokenExpireDay), claims);
    }

    public static ClaimsPrincipal GetPrincipalFromExpiredToken(JwtSettings settings, string token,
                                                               Audiences audience = Audiences.Public)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = audience.ToString(),
            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSecret)),
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                StringComparison.InvariantCulture))
            throw new SecurityTokenException("Invalid Token");

        return principal;
    }

    public static ClaimsPrincipal GetPrincipalFromToken(JwtSettings settings, string token,
                                                               Audiences audience = Audiences.Public)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = audience.ToString(),
            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSecret)),
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                StringComparison.InvariantCulture) || jwtSecurityToken.ValidTo <= DateTime.UtcNow)
                throw new SecurityTokenException("Invalid Token");

            return principal;

        }
        catch (Exception)
        {

            throw new SecurityTokenException("Invalid Token");

        }
    }

    public static bool IsExpired(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken)
            return true;

        var validTo = jwtToken.ValidTo;
        if (validTo == DateTime.MinValue) return false;
        return validTo <= DateTime.UtcNow;
    }


}

public class TokenModel
{
    public string Id { get; }
    public string Name { get; }
    public string Email { get; }
    public List<string> Roles { get; set; }


    public TokenModel(ClaimsPrincipal claimsPrincipal)
    {
        Id = claimsPrincipal.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value;
        Roles = claimsPrincipal.Claims.Where(p => p.Type == ClaimTypes.Role).Select(p => p.Value).ToList();
        Name = claimsPrincipal.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value ?? "noname";
        Email = claimsPrincipal.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value ?? "";
        //foreach (var team in claimsPrincipal.Claims.Where(p => p.Type == "Teams")?.Select(p => p.Value).ToList())
        //{
        //    TeamIds.Add(Guid.Parse(team));
        //}
        //TeamIds = claimsPrincipal.Claims.Where(p => p.Type == "Teams").Select(p => p.Value)?.ToList(); ToDO

    }
    public static IEnumerable<Claim> GetClaims(CurrentUser account)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.UserId),
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Email, account.UserMail),
            };
        foreach (var item in account.Role)
        {
            claims.Add(new Claim(ClaimTypes.Role, item));
        }
        //account.TeamIds = new List<Guid>();// temp
        //account.TeamIds.Add(Guid.NewGuid()); // temp
        //if (account.TeamIds != null)
        //{
        //    foreach (var item in account.TeamIds)
        //    {
        //        claims.Add(new Claim("Teams", item.ToString()));
        //    }
        //}
        return claims;
    }
}

public enum Audiences
{
    Public
}
