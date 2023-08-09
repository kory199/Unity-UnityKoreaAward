using APIServer.DbModel;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using ZLogger;

namespace APIServer.Services;

public class MasterDataDb : BaseDb<MasterDataDic>, IMasterDataDb
{
    public MasterDataDb(ILogger<MasterDataDb> logger, IOptions<DbConfig> dbConfig)
        : base(logger, dbConfig.Value.MasterDataDb, MasterDataTable.game_ver)
    {
    }

    public async Task<ResultCode> LoadAllMasterDataAsync()
    {
        try
        {
            var resultCode = await VerifyMonsterDataAsync();
            if (resultCode != ResultCode.None) return resultCode;

            resultCode = await VerifyPlayerStatusAsync();
            if (resultCode != ResultCode.None) return resultCode;

            resultCode = await VerifyStageSpawnAsync();
            if (resultCode != ResultCode.None) return resultCode;

            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, "Error loading master data.");
            return ResultCode.LoadMasterDataFailException;
        }
    }

    public async Task<(ResultCode, String?)> VerifyGmaeVersionAsync()
    {
        var gameVer = await _queryFactory.Query(_tableName)
            .Select(MasterDataTable.version)
            .OrderByDesc(MasterDataTable.version)
            .FirstOrDefaultAsync<Int32>();

        if (gameVer == 0)
        {
            return (ResultCode.LoadGameVersionFail, null);
        }

        String verStr = VersionStr(gameVer);
        return (ResultCode.None, verStr);
    }

    private String VersionStr(Int32 verNum)
    {
        string versionStr = $"{verNum / 10000}.{(verNum % 10000) / 1000}.{verNum % 10}";
        return versionStr;
    }

    public async Task<ResultCode> VerifyMonsterDataAsync()
    {
        try
        {
            var monsterdata = await _queryFactory.Query(MasterDataTable.monster_data)
            .GetAsync<MonsterData>();

            if (monsterdata == null)
            {
                return ResultCode.LoadMasterMonsterDataFail;
            }

            List<MonsterData> meleeMonsters = new List<MonsterData>();
            List<MonsterData> rangedMonsters = new List<MonsterData>();
            MonsterData bossMonster = null;

            foreach (var monster in monsterdata)
            {
                switch (monster.type)
                {
                    case "MeleeMonster":
                        meleeMonsters.Add(monster);
                        break;
                    case "RangedMonster":
                        rangedMonsters.Add(monster);
                        break;
                    case "BOSS":
                        bossMonster = monster;
                        break;
                }
            }

            MasterDataDic.masterDataDic.Add(MasterDataDicKey.MeleeMonster, meleeMonsters);
            MasterDataDic.masterDataDic.Add(MasterDataDicKey.RangedMonster, rangedMonsters);
            MasterDataDic.masterDataDic.Add(MasterDataDicKey.BOSS, bossMonster);

            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.VerifyMonsterDataAsync] ResultCode : {ResultCode.LoadMasterMonsterDataFailException}");

            return ResultCode.LoadMasterMonsterDataFailException;
        }
    }

    public async Task<ResultCode> VerifyPlayerStatusAsync()
    {
        try
        {
            var playerStatus = await _queryFactory.Query(MasterDataTable.player_status)
                .GetAsync<PlayerStatus>();

            if (playerStatus == null)
            {
                return ResultCode.LoadPlayerStatusDataFail;
            }

            MasterDataDic.masterDataDic.Add(MasterDataDicKey.PlayerStatus, playerStatus.ToList());
            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.VerifyPlayerStatusAsync] ResultCode : {ResultCode.LoadPlayerStatusDataFailException}");

            return ResultCode.LoadPlayerStatusDataFailException;
        }
    }

    public async Task<ResultCode> VerifyStageSpawnAsync()
    {
        try
        {
            var spawnMonster = await _queryFactory.Query(MasterDataTable.stage_spawn_monster)
                .GetAsync<StageSpawnMonsterData>();

            if (spawnMonster == null)
            {
                return ResultCode.LoadStageSpawnMonsterDataFail;
            }

            MasterDataDic.masterDataDic.Add(MasterDataDicKey.StageSpawnMonster, spawnMonster.ToList());
            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.VerifyStageSpawnAsync] ResultCode : {ResultCode.LoadStageSpawnMonsterDataFailException}");

            return ResultCode.LoadStageSpawnMonsterDataFailException;
        }
    }
}