using APIServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APIServer.ReqResModel;
using APIServer.DbModel;

namespace APIServer.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected readonly ILogger<BaseApiController> _logger;
    protected readonly IAccountDb _accountDb;
    protected readonly IMemoryDb _memoryDb;

    protected BaseApiController(ILogger<BaseApiController> logger, IMemoryDb memoryDb, IAccountDb accountDb)
    {
        _logger = logger;
        _memoryDb = memoryDb;
        _accountDb = accountDb;
    }

    protected T CreateResponse<T>(ResultCode resultCode) where T : BaseResponse, new()
    {
        return new T
        {
            Result = resultCode,
            ResultMessage = resultCode.ToString()
        };
    }
}