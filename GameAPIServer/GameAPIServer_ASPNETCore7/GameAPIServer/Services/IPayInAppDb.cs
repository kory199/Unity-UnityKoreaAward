using System;
using GameAPIServer.DBModel;
using GameAPIServer.StateType;

namespace GameAPIServer.Services;

public interface IPayInAppDb
{
    public Task<ErrorCode> CheckReceiptAsync(Int64 uniqueKey, Int64 receiptNo, Int32 code);

    public Task<Tuple<ErrorCode, List<Mail>>> InsertReceiptDataAsync(Int64 uniqueKey, Int64 date, Int64 receipteNo, Int32 code);

    public Task<Mail> SandInAppMailAsync(Int64 uniquekey, Int32 itemCode, Int64 count);
}