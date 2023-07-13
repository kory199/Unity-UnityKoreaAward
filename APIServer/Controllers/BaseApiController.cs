using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected readonly ILogger<BaseApiController> _logger;
    protected readonly IMemoryDb _memoryDb;

    protected BaseApiController(ILogger<BaseApiController> logger, IMemoryDb memoryDb)
    {
        _logger = logger;
        _memoryDb = memoryDb;
    }
}