using System;
using GameAPIServer.DBModel;

namespace GameAPIServer.Services;

public interface IDungeonStage
{
    public Task<ErrorCode> CreateStageData(Int64 userId, Int64 stageid);

    public Task<IEnumerable<DungeonStage>> LoadStageDataAsync(Int64 userId);

    public Task<ErrorCode> GetStageItem(Int64 stage_id);

    public List<DungeonStage> GetList();

    public List<Item> GetItemList();

    public List<StageAttackNpc> GetNPCList();
}