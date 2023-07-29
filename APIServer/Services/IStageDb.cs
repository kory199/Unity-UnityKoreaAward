﻿using APIServer.DbModel;

namespace APIServer.Services;

public interface IStageDb
{
    public Task<(ResultCode, Stage?)> CreateDefaultStageData(Int64 account_id);

    public Task<(ResultCode, Stage?)> CreateStageData(Int64 account_id, Int32 stage_id);

    public Task<(ResultCode, List<Stage>?)> VerifyStageAsync(Int64 account_id);

    public Task<(ResultCode, Stage?)> UpdataStageAsync(Int64 account_id, Int32 stage_id );
}