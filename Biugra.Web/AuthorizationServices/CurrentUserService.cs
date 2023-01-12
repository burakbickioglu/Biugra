using Biugra.Service.Utilities;
using Biugra.Service.Utilities.Helpers;

namespace Biugra.Web.AuthorizationServices
{
    public class CurrentUserService : CurrentUserServiceBase
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, JwtSettings settings) : base(httpContextAccessor, settings)
        {
            TokenModel = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true ? new TokenModel(_httpContextAccessor.HttpContext.User) : null;
        }

        public override void Authenticate(string token)
        {
            try
            {
                TokenModel = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true ? new TokenModel(_httpContextAccessor.HttpContext.User) : null;
            }
            catch (Exception)
            {
            }
        }

        public override string GetToken()
        {
            return IsAuthenticated()
                ? JwtHelper.CreateJwtToken(_settings, Audiences.Public, _httpContextAccessor.HttpContext.User.Claims)
                : string.Empty;
        }
        //public override bool IsAuthenticated() => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;


        public override bool IsAuthenticated() => base.IsAuthenticated() || (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false);
    }
}
