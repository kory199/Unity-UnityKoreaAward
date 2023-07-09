using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameAPIServer.Services;
using GameAPIServer.ReqResModel;
using GameAPIServer.DBModel;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class DungeonStageController : ControllerBase
{
    private readonly IDungeonStage _dungeonStage;
    private readonly IMemoryDb _memoryDb;
    private readonly ILogger<DungeonStageController> _logger;

    public DungeonStageController(ILogger<DungeonStageController> logger,IMemoryDb memoryDb, IDungeonStage dungeonStage)
    {
        _logger = logger;
        _dungeonStage = dungeonStage;
        _memoryDb = memoryDb;
    }

    [HttpPost]
    public async Task<PkDungeonStageResPonse> Post (PkRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;

        var response = new PkDungeonStageResPonse();

        var error = await _dungeonStage.LoadStageDataAsync(userInfo.AccountId);

        response.mailList = DungeonStageDb.DungeonStagesList;
        
        return response;
    }
}