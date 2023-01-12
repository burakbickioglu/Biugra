
namespace Biugra.Service.Services;

public class CurrentUserService : ICurrentUserService
{
    public readonly IHttpContextAccessor _httpContextAccessor;
    public TokenModel? TokenModel;
    public JwtSettings _settings { get; set; }
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, JwtSettings settings)
    {
        _httpContextAccessor = httpContextAccessor;
        _settings = settings;
    }

    public void Authenticate(string token)
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

    public List<string> GetRoles() => this.IsAuthenticated() ? TokenModel!.Roles : new List<string>();

    public string GetToken() // ToDo
    {
        if (!this.IsAuthenticated())
            return string.Empty;

        var currentUser = new CurrentUser(GetUserId(), GetUserName(), GetUserEmail(), GetRoles());
        var stringToken = JwtHelper.CreateJwtToken(_settings, Audiences.Public, TokenModel.GetClaims(currentUser));

        return stringToken;
    }

    //public List<Guid>? GetTeamIds() => this.IsAuthenticated() ? TokenModel!.TeamIds : null;

    public string GetUserEmail() => this.IsAuthenticated() ? TokenModel!.Email : string.Empty;

    public string GetUserId() => this.IsAuthenticated() ? TokenModel!.Id : string.Empty; // try guid parse işlemi yapılacak (guid tipinde)

    public string GetUserName() => this.IsAuthenticated() ? TokenModel!.Name : string.Empty;

    public bool IsAuthenticated()
    {
        if (TokenModel != null)
            return true;

        if (_httpContextAccessor.HttpContext?.Request?.Headers?.ContainsKey("Authorization") == true)
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token))
            {
                Authenticate(token);

            }
        }

        return TokenModel != null;
    }

    public bool Logout()
    {
        TokenModel = null;
        return true;
    }

    public string GetIpAddress()
    {
        throw new NotImplementedException();
    }
}
