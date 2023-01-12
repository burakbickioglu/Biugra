

namespace Biugra.Service.Middlewares;

public class ApiAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiAuthMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Invoke(HttpContext context, ICurrentUserService userService)
    {
        var token = context?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
            userService.Authenticate(token);
        else
            userService.Logout();

        await _next(context);
    }
}
