namespace Biugra.Application.Interfaces;

public interface ITokenHandler
{
    Token CreateAccessToken(int day);
}
