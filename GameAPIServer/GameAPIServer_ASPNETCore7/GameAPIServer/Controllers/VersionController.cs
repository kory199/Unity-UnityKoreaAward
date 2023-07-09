using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController : ControllerBase
{
    private readonly IVerDb _verDb;
    private readonly ILogger<VersionController> _logger;

    public VersionController(ILogger<VersionController> logger, IVerDb verDb)
    {
        _logger = logger;
        _verDb = verDb;
    }

    [HttpPost]
    public async Task<PkVersionResponse> Post(PkVersionRequset request)
    {
        var response = new PkVersionResponse();

        var errorCode= await _verDb.VerifyMasterDataVer();

        errorCode = await _verDb.VerifyGameVer();
        //Console.WriteLine($"masterVer : {masterVer}");
        //Console.WriteLine($"gameVer : {gameVer}");

        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.LoadVersion], $"Load Version Success");

        response.MasterDataVer = _verDb.GetMasterDataVersion();
        response.GameVer = _verDb.GetGameVersion();

        return response;
    }
}