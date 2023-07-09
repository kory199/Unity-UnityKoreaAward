using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class SelectedStageController : ControllerBase
{
    private readonly IDungeonStage _dungeonStage;
    private readonly IMemoryDb _memoryDb;
    private readonly ILogger<SelectedStageController> _logger;

    public SelectedStageController(ILogger<SelectedStageController> logger, IDungeonStage dungeonStage, IMemoryDb memoryDb)
    {
        _logger = logger;
        _dungeonStage = dungeonStage;
        _memoryDb = memoryDb;
    }

    [HttpPost]
    public async Task<PkSelectedStagedResPonse> Post (PkSelectedStagedRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var response = new PkSelectedStagedResPonse();


        var errorcode = await _memoryDb.ChangegedUserState(userInfo.Id);

        var itemList = _dungeonStage.GetItemList();

        List <StageItemInfo> loaditem = itemList.Select(m => new StageItemInfo
        {
            item_code = m.code,
            count = 1
        }).ToList();

        response.StageList = loaditem;
        response.StageNPCList = _dungeonStage.GetNPCList();

        errorcode = await _memoryDb.RegistUserItemAsync(userInfo.Id);
        errorcode = await _memoryDb.RegistUerNPCAsync(userInfo.Id);

        if (errorcode != ErrorCode.None)
        {
            response.Result = errorcode;
            return response;
        }

        return response;
    }
}