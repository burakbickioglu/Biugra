
namespace Biugra.Service.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string? message = "Unouthorized") : base(message) { }
}
