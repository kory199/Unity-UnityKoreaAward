using System.Text;
using System.Text.Json;
using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;

namespace GameAPIServer.Middleware;

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

         if (string.Compare(formString, "/Version", StringComparison.OrdinalIgnoreCase) == 0||
			string.Compare(formString, "/Login", StringComparison.OrdinalIgnoreCase) == 0 ||
        	string.Compare(formString, "/CreateAccount", StringComparison.OrdinalIgnoreCase)== 0)
        {
			await _next(context);

            return;
		}

		context.Request.EnableBuffering();
		
		string AuthToken;
		string Id;
		string userLockKey = "";

		using(var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 4096, true))
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
			var (issame, userVersionInfo) = await _memoryDb.GetUserVersion(Id);

			if(isOk == false || issame == false)
			{
				return;
			}

			if(await IsInvalidUserAuthTokenThenSendError(context, userInfo, AuthToken))
			{
				return;
			}

			if(await IsInvalidUserVersionThenSendError(context, userInfo, userVersionInfo))
			{
				return;
			}

			userLockKey = MemoryDbKeyMaker.MakeUserLockKey(userInfo.Id);

			if(await SetLoackAndIsFailThenSendError(context, AuthToken))
			{
				return;
			}

			context.Items[nameof(AuthUser)] = userInfo; 
		}

		context.Request.Body.Position = 0;  

        await _next(context);

		await _memoryDb.DelUserReqLockAsync(userLockKey);
	}

    private async Task<bool> IsNullBodyDataThenSendError(HttpContext context, string bodyStr)
	{
        if (string.IsNullOrEmpty(bodyStr) == false) 
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

	private bool IsInvalidJsonFormatThenSendError(HttpContext context, JsonDocument document, out string id, out string authToken)
	{
		try
		{
			id = document.RootElement.GetProperty("Id").GetString(); 

			authToken = document.RootElement.GetProperty("AuthToken").GetString();
            
            return false;
		}
		catch
		{
			id = "";
			authToken = "";

			var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
			{
				Result = ErrorCode.AuthTokenFailWrongKeyword
			});

			var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);

			context.Response.Body.Write(bytes, 0, bytes.Length); 

			return true;
		}
	}

    private static async Task<bool> IsInvalidUserAuthTokenThenSendError(HttpContext context, AuthUser userInfo, string authToken)
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

	private static async Task<bool> IsInvalidUserVersionThenSendError(HttpContext context, AuthUser userVersionInfo, String userVersion)
	{
		if(userVersionInfo.UserVersion == userVersion)
		{
			return false;
		}

		var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
		{
			Result = ErrorCode.VerifyVersionException
		});

		var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);

        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);

        return true;
    }

    private async Task<bool> SetLoackAndIsFailThenSendError(HttpContext context, string AuthToken)
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