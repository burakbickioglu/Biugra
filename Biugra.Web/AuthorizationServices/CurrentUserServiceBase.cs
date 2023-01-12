using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities;
using Biugra.Service.Utilities.Helpers;

namespace Biugra.Web.AuthorizationServices;

public class CurrentUserServiceBase : ICurrentUserService
{
    public readonly IHttpContextAccessor _httpContextAccessor;
    public readonly JwtSettings _settings;
    public TokenModel? TokenModel;
    public string? ApiToken;
    public CurrentUserServiceBase(IHttpContextAccessor httpContextAccessor, JwtSettings settings)
    {
        _httpContextAccessor = httpContextAccessor;
        _settings = settings;
    }
    public virtual void Authenticate(string token)
    {
        try
        {
            var model = JwtHelper.GetPrincipalFromToken(_settings, token);
            TokenModel = new TokenModel(model);
        }
        catch (Exception)
        {
        }
    }

    public string GetIpAddress() => _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
    public virtual string GetToken() => string.Empty;


    public string GetUserId() => IsAuthenticated() ? TokenModel!.Id : string.Empty;
    public string GetUserName() => IsAuthenticated() ? TokenModel!.Name : string.Empty;


    public virtual bool IsAuthenticated() => TokenModel != null;

    public string? GetApiToken() => ApiToken;

    public void UpdateApiToken(string token) => ApiToken = token;
    public bool Logout()
    {
        TokenModel = null;
        return true;
    }

    public string GetUserEmail() => IsAuthenticated() ? TokenModel!.Email : string.Empty;

    public List<string> GetRoles()
    {
        throw new NotImplementedException();
    }
}
