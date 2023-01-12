
namespace Biugra.Service.Utilities;

public static class GoogleRecaptchaHelper
{
    // A function that checks reCAPTCHA results
    public static async Task<bool> IsReCaptchaPassedAsync(string gRecaptchaResponse, string secret)
    {
        //if (string.IsNullOrEmpty(gRecaptchaResponse))
        //    return true;

        HttpClient httpClient = new HttpClient();
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("secret", secret),
            new KeyValuePair<string, string>("response", gRecaptchaResponse)
        });
        var res = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify", content);
        if (res.StatusCode != HttpStatusCode.OK)
        {
            return false;
        }

        string JSONres = res.Content.ReadAsStringAsync().Result;
        dynamic JSONdata = JObject.Parse(JSONres);
        if (JSONdata.success != "true")
        {
            return false;
        }

        return true;
    }
}

