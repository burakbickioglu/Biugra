

namespace Biugra.Service.Utilities.Helpers
{
    public interface IGoogleLoginHelper
    {
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDTO externalAuth);
    }
}
