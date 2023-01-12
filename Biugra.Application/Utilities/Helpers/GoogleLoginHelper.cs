
namespace Biugra.Service.Utilities.Helpers;

public class GoogleLoginHelper : IGoogleLoginHelper
{
    public IConfigurationSection _googleSettings { get; set; }

    public GoogleLoginHelper(IConfiguration configuration)
    {
        _googleSettings = configuration.GetSection("GoogleAuthSettings");
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDTO externalAuth)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _googleSettings.GetSection("cliendId").Value }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.TokenId, settings);
            return payload;
        }
        catch (Exception ex)
        {
            //log an exception
            return null;
        }
    }
}
