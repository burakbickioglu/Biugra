using Biugra.Domain.Models.Dtos;
using System.Data;

namespace Biugra.Service.Utilities.Helpers;

public static class HttpClientExtensions
{
    public static async Task<T?> ReadBaseContentAs<T>(this HttpResponseMessage response) where T : class
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return null;

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return null;

            var obj = JObject.Parse(dataAsString);

            if (obj is null)
                return null;

            var result = obj.ToObject<T>();

            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static async Task<List<T>> ReadBaseStructContentAsArray<T>(this HttpResponseMessage response) where T : struct
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return null;

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return null;

            var obj = JArray.Parse(dataAsString);

            if (obj is null)
                return null;

            var result = obj.ToObject<T[]>();

            return result.ToList();
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static async Task<List<T>> ReadBaseContentAsArray<T>(this HttpResponseMessage response) where T : class
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return null;

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return null;

            var obj = JArray.Parse(dataAsString);

            if (obj is null)
                return null;

            var result = obj.ToObject<T[]>();

            return result.ToList();
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static async Task<CommandResult<T>> ReadContentAs<T>(this HttpResponseMessage response) where T : class
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return CommandResult<T>.GetFailed(response.ReasonPhrase ?? "");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return CommandResult<T>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE);

            var obj = JObject.Parse(dataAsString);

            if (obj is null)
                return CommandResult<T>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE);

            // obj.ToObject<CommandResult<T>>();

            var dataobj = obj["data"] ?? obj["Data"];
            var messageobj = obj["message"] ?? obj["Message"];
            var issuceedobj = obj["isSucceed"] ?? obj["IsSucceed"];

            var issucceed = issuceedobj?.ToObject<bool>() ?? false;

            if (issucceed && dataobj is not null)
                return CommandResult<T>.GetSucceed(messageobj?.ToString() ?? string.Empty, dataobj.ToObject<T>());


            return CommandResult<T>.GetFailed(messageobj?.ToString() ?? string.Empty);
        }
        catch (Exception ex)
        {
            return CommandResult<T>.GetFailed(ex);
        }
    }
    //public static async Task<CommandResult<DataTable>> ReadContentAsDataTable(this HttpResponseMessage response)
    //{
    //    try
    //    {
    //        if (!response.IsSuccessStatusCode)
    //            return CommandResult<DataTable>.GetFailed(response.ReasonPhrase ?? "");

    //        var dataAsString = await response.Content.ReadAsStringAsync();

    //        if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
    //            return CommandResult<DataTable>.GetFailed("");

    //        var obj = JObject.Parse(dataAsString);

    //        if (obj is null)
    //            return CommandResult<DataTable>.GetFailed("");

    //        var dataobj = obj["data"];
    //        var messageobj = obj["message"];
    //        var issuceedobj = obj["isSucceed"];

    //        var issucceed = issuceedobj?.ToObject<bool>() ?? false;

    //        if (!issucceed)
    //            return CommandResult<DataTable>.GetFailed(messageobj?.ToObject<string>());

    //        var data = dataobj.ToObject<DataTableApiResponse>();

    //        if (data is null)
    //            return CommandResult<DataTable>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE);

    //        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(data.DataSourceRequest);

    //        DataTable dataTable = dataSet.Tables[0];

    //        return dataTable == null ?
    //            CommandResult<DataTable>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE) :
    //            CommandResult<DataTable>.GetSucceed(dataTable);

    //    }
    //    catch (Exception ex)
    //    {
    //        return CommandResult<DataTable>.GetFailed(ex);
    //    }
    //}

    //private static string GetUriWithQueryString(string requestUri,
    //        Dictionary<string, string> queryStringParams)
    //{
    //    bool startingQuestionMarkAdded = false;
    //    var sb = new StringBuilder();
    //    sb.Append(requestUri);
    //    foreach (var parameter in queryStringParams)
    //    {
    //        if (parameter.Value == null)
    //        {
    //            continue;
    //        }

    //        sb.Append(startingQuestionMarkAdded ? '&' : '?');
    //        sb.Append(parameter.Key);
    //        sb.Append('=');
    //        sb.Append(parameter.Value);
    //        startingQuestionMarkAdded = true;
    //    }
    //    return sb.ToString();
    //}
}
