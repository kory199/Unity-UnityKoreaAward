using System;
using Newtonsoft.Json;

public class APIResponse<T>
{
    public int Result { get; set; }
    public string ResultMessage { get; set; }
    public T Data { get; set; }

    public static APIResponse<T> FromJson(string responseBodyJson)
    {
        var response = JsonConvert.DeserializeObject<APIResponse<T>>(responseBodyJson);

        if(response.Result != 0)
        {
            throw new Exception($"API Response Error. Result: {response.ResultMessage}");
        }

        return response;
    }
}