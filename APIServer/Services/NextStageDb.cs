using APIServer.DbModel;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using ZLogger;

namespace APIServer.Services;

public class NextStageDb : BaseDb<StageInfo>, INextStage
{
    public static Dictionary<int, int> StageInfoDic = new Dictionary<int, int>();

    public NextStageDb(ILogger<NextStageDb> logger, IOptions<DbConfig> dbConfig)
        : base(logger, dbConfig.Value.GameDb, StageInfoTable.stage)
    {
    }

    public async Task<ResultCode> LoadStageDataAsync()
    {
        try
        {
            var stageInfo = await _queryFactory.Query(_tableName)
                .GetAsync();

            if(stageInfo == null)
            {
                return ResultCode.LoadStageInfoFail;
            }

            foreach(var stage in stageInfo)
            {
                StageInfoDic.Add(stage.stage_id, stage.prev_stage_id);
            }

            return ResultCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.LoadStageDataAsync] ErrorCode : {ResultCode.LoadStageInfoFailException}");
            return ResultCode.LoadStageInfoFailException;
        }
    }
}