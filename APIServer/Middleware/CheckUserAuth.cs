using APIServer.DbModel;
using APIServer.ReqResMondel;
using APIServer.Services;
using System.Text;
using System.Text.Json;

namespace APIServer.Middleware;

public class CheckUserAuth
{
    private readonly IMemoryDb _memoryDb;
    private readonly RequestDelegate _next;

    public CheckUserAuth(RequestDelegate next, IMemoryDb memoryDb)
    {
        _memoryDb = memoryDb;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var formString = context.Request.Path.Value;

        if (string.Compare(formString, "/Login", StringComparison.OrdinalIgnoreCase) == 0 ||
           string.Compare(formString, "/CreateAccount", StringComparison.OrdinalIgnoreCase) == 0)
        {
            await _next(context);
            return;
        }

        context.Request.EnableBuffering();

        string AuthToken = "";
        string Id = "";
        string userLockKey = "";

        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 4096, true))
        {
            var bodyStr = await reader.ReadToEndAsync();

            if (await IsNullBodyDataThenSendError(context, bodyStr))
            {
                return;
            }

            var document = JsonDocument.Parse(bodyStr);

            if (IsInvalidJsonFormatThenSendError(context, document, out Id, out AuthToken))
            {
                return;
            }

            var (isOk, userInfo) = await _memoryDb.GetUserAsync(Id);

            if (isOk == false)
            {
                return;
            }

            if(await IsInvalidUserAuthTokenThenSendError(context, userInfo, AuthToken))
            {
                return;
            }

            userLockKey = MemoryDbKeyMaker.MakeUserLockKey(userInfo.Id);

            if(await SetLoackAndIsFailThenSEndError(context, AuthToken))
            {
                return;
            }

            context.Items[nameof(AuthUser)] = userInfo;
        }

        context.Request.Body.Position = 0;
        await _next(context);
        await _memoryDb.DelUserReqLockAsync(userLockKey);
    }

    private async Task<bool> IsNullBodyDataThenSendError(HttpContext context, String bodyStr)
    {
        if (string.IsNullOrWhiteSpace(bodyStr) == false)
        {
            return false;
        }

        var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            Result = ErrorCode.InValidRequestHttpBody
        });

        var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        return true;
    }

    private bool IsInvalidJsonFormatThenSendError(HttpContext context, JsonDocument document, out String id, out String authToken )
    {
        try
        {
            id = document.RootElement.GetProperty("ID").GetString();
            authToken = document.RootElement.GetProperty("AUthToken").GetString();
            return false;
        }
        catch
        {
            id = "";
            authToken = "";

            var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
            {
                Result = ErrorCode.AuthTokenFailWrongAuthToken
            });

            var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
            context.Response.Body.Write(bytes, 0, bytes.Length);
            return true;
        }
    }

    private static async Task<bool> IsInvalidUserAuthTokenThenSendError(HttpContext context, AuthUser userInfo, String authToken)
    {
        if(string.CompareOrdinal(userInfo.AuthToken, authToken) == 0)
        {
            return false;
        }

        var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            Result = ErrorCode.AuthTokenFailWrongAuthToken
        });

        var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        return true;
    }

    private async Task<bool> SetLoackAndIsFailThenSEndError(HttpContext context, String AuthToken)
    {
        if(await _memoryDb.SetUserReqLockAsync(AuthToken))
        {
            return false;
        }

        var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            Result = ErrorCode.AuthTokenFailSetNx
        });

        var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        return true;
    }
}