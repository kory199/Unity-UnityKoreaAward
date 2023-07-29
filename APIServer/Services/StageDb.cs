using APIServer.DbModel;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using System.Diagnostics;
using ZLogger;

namespace APIServer.Services;

public class StageDb : BaseDb<Stage>, IStageDb
{
    public StageDb(ILogger<StageDb> logger, IOptions<DbConfig> dbConfig)
        : base(logger, dbConfig.Value.GameDb, StageTable.player_stage)
    {
    }

    public async Task<(ResultCode, Stage?)> CreateDefaultStageData(Int64 account_id)
    {
        try
        {
            var defaultStage = new Stage
            {
                player_uid = account_id,
                stage_id = 1,
                is_achieved = false,
            };

            var count = await ExecuteInsertAsync(defaultStage);

            if (count == 0)
            {
                return (ResultCode.CreateDefaultStageFailInsert, null);
            }

            return (ResultCode.None, defaultStage);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
               $"[{GetType().Name}.CreateDefaultStageData] ErrorCode : {ResultCode.CreateDefaultStageFailException}");

            return (ResultCode.CreateDefaultStageFailException, null);
        }
    }

    public async Task<(ResultCode, Stage?)> CreateStageData(Int64 account_id, Int32 stage_id)
    {
        try
        {
            var newStage = new Stage
            {
                player_uid = account_id,
                stage_id = stage_id,
                is_achieved = false,
            };

            var count = await ExecuteInsertAsync(newStage);

            if (count == 0)
            {
                return (ResultCode.CreateDefaultStageFailInsert, null);
            }
            return (ResultCode.None, newStage);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
               $"[{GetType().Name}.CreateDefaultStageData] ErrorCode : {ResultCode.CreateDefaultStageFailException}");

            return (ResultCode.CreateDefaultStageFailException, null);
        }
    }

    public async Task<(ResultCode, List<Stage>?)> VerifyStageAsync(Int64 account_id)
    {
        try
        {
            var stageList = await ExecutGetByListAsync(ColumnUid.player_uid, account_id);

            if(stageList == null)
            {
                return (ResultCode.LoadStageDataNotFound, null);
            }

            return (ResultCode.None, stageList);
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[{GetType().Name}.VerifyStageAsync] ErrorCode : {ResultCode.LoadStageDataFailException}");

            return (ResultCode.LoadStageDataFailException, null);
        }
    }

    public async Task<(ResultCode, Stage?)> UpdataStageAsync(Int64 account_id, Int32 stage_id)
    {
        try
        {
            var stageClear = await _queryFactory.Query(_tableName)
                .Where(ColumnUid.player_uid, account_id)
                .UpdateAsync(new { is_achieved = true });

            if(stageClear == 0)
            {
                return (ResultCode.UpdateStageDataFail, null);
            }

            var getNextStage = NextStageDb.StageInfoDic[stage_id];
            var ( resultcode, addNextStage) = await CreateStageData(account_id, getNextStage);

            if(addNextStage == null)
            {
                return (ResultCode.UpdateStageDataFail, null);
            }

            return (ResultCode.None, addNextStage);
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                 $"[{GetType().Name}.UpdataStageAsync] ErrorCode : {ResultCode.UpdateStageDataFailException}");

            return (ResultCode.UpdateStageDataFailException, null);
        }
    }
}