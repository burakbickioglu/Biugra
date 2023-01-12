

namespace Biugra.Service.Behaviours;

public class RequestAuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {

        var requestType = request.GetType();
        var attributes = requestType.GetCustomAttributes(typeof(IAuthorizationAttribute), true);
        if (attributes.Length == 0)
        {
            return await next();
        }

        var roleAttribute = attributes.FirstOrDefault(x => x.GetType() == typeof(RoleAuthorizationAttribute)) as RoleAuthorizationAttribute;
        //var accountTypeAttribute = attributes.FirstOrDefault(x => x.GetType() == typeof(AccountTypeAuthorizationAttribute)) as AccountTypeAuthorizationAttribute;

        var validRoles = roleAttribute?.ValidRoles;
        //var validTypes = accountTypeAttribute?.ValidTypes;

        if (validRoles == null)
        {
            return await next();
        }

        if (_currentUserService.IsAuthenticated())
        {
            //if (_currentUserService.GetAccountType() == AccountType.Consumer)
            //    return await next();

            var userRoles = _currentUserService.GetRoles();

            var rolePass = validRoles == null || userRoles.Contains(Domain.Enums.Roles.Admin.ToString()) || userRoles.Contains(Domain.Enums.Roles.Admin.ToString());

            //var typePass = validTypes == null || (validTypes != null && validTypes.Any(e => e == _currentUserService.GetAccountType()));

            //if (rolePass && typePass)

            if (rolePass)
            {
                return await next();
            }
        }
        _logger.LogWarning("REQUEST AUTHORIZATION ERROR --- RequestType : {RequestType} -- UserId : {UserId} --  Request : {@Request}", typeof(TRequest), _currentUserService.GetUserId(), request);

        throw new UnauthorizedException();
    }

    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<TRequest> _logger;
    public RequestAuthorizationBehaviour(ICurrentUserService currentUserService, ILogger<TRequest> logger)
    {
        _currentUserService = currentUserService;
        _logger = logger;
        var roles = _currentUserService.GetRoles();
        var token = _currentUserService.GetToken();
        var username = _currentUserService.GetUserName();
        var isauth = _currentUserService.IsAuthenticated();
    }
}
