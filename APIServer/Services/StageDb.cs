using APIServer.DbModel;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
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

            var result = await ExecuteInsertAsync(defaultStage);

            if (result != ResultCode.None)
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

    public async Task<(ResultCode, Int32?)> VerifyStageAsync(Int64 account_id)
    {
        try
        {
            Int32? stageNum = await _queryFactory.Query(StageTable.player_stage)
            .Where(GameDbTable.player_uid, account_id)
            .OrderByDesc(StageTable.stage_id)
            .Select(StageTable.stage_id)
            .FirstOrDefaultAsync<Int32?>();

            if (!stageNum.HasValue || stageNum.Value == 0)
            {
                return (ResultCode.LoadStageDataNotFound, null);
            }

            return (ResultCode.None, stageNum);
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[{GetType().Name}.VerifyStageAsync] ErrorCode : {ResultCode.LoadStageDataFailException}");

            return (ResultCode.LoadStageDataFailException, null);
        }
    }

    public async Task<ResultCode> UpdataStageAsync(Int64 account_id, Int32 stage_id)
    {
        try
        {
            var stageClear = await _queryFactory.Query(_tableName)
                .Where(ColumnUid.player_uid, account_id)
                .Where(StageTable.stage_id, stage_id)
                .UpdateAsync(new { 
                    is_achieved = true,
                });

            if(stageClear == 0 && stage_id >= 5)
            {
                return ResultCode.UpdateStageDataFail;
            }

            var getNextStage = NextStageDb.StageInfoDic[stage_id];
            var newStage = new Stage
            {
                player_uid = account_id,
                stage_id = getNextStage,
                is_achieved = false,
            };

            var result = await ExecuteInsertAsync(newStage);

            if(result != ResultCode.None)
            {
                return ResultCode.InertNewStageDatatFail;
            }

            return ResultCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                 $"[{GetType().Name}.UpdataStageAsync] ErrorCode : {ResultCode.UpdateStageDataFailException}");

            return ResultCode.UpdateStageDataFailException;
        }
    }
}