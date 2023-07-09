using System.Data;
using GameAPIServer.DBModel;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata;
using SqlKata.Execution;
using ZLogger;

namespace GameAPIServer.Services;

public class PayInAppDb : IPayInAppDb
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<PayInAppDb> _logger;

    private IDbConnection _dbConn;
    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    public PayInAppDb(ILogger<PayInAppDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<ErrorCode> CheckReceiptAsync(Int64 uniqueKey, Int64 receiptNo, Int32 code)
    {
        try
        {
            var newReceipe = Receipt.NewReceipt();

            if (newReceipe.code != code || newReceipe.receiptNo != receiptNo)
            {
                _logger.ZLogError( $"[PayInAppDb.CheckReceiptAsync] ErrorCode : {ErrorCode.ReceiptException}, receiptNo :{receiptNo}");

                return ErrorCode.ReceiptException;
            }

            var checkCount = await GetCountByReceiptNoAsync(receiptNo);

            if(checkCount >= 3)
            {
                _logger.ZLogError($"[PayInAppDb.GetCountByReceiptNoAsync] ErrorCode : {ErrorCode.ReceiptDulicateAuthenticationException}, receiptNo :{receiptNo}");

                return ErrorCode.ReceiptException;
            }

            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,$"[PayInAppDb.CheckReceiptAsync] ErrorCode : {ErrorCode.ReceiptException}, receiptNo :{receiptNo}");

            return ErrorCode.ReceiptException;
        }
    }

    public async Task<Tuple<ErrorCode, List<Mail>>> InsertReceiptDataAsync(Int64 userId, Int64 date, Int64 receipteNo, Int32 code)
    {
        try
        {
            List<Mail> sendMailList = new List<Mail>();

            int startIndex = (code - 1) * 3;
            int endIndex = startIndex + 3;

            if (code == 3) endIndex = 10;

            for (int i = startIndex; i < endIndex; i++)
            {
                var itemCode = MasterDataDb.inAppProductList[i].item_code;
                var count = MasterDataDb.inAppProductList[i].item_count;

                var item = await _queryFactory.Query(GameDbTable.inapp).InsertAsync(new
                {
                    player_id = userId,
                    receipt_number = receipteNo,
                    item_code = itemCode,
                    count = count
                });

                Mail sandNewMail = await SandInAppMailAsync(userId, itemCode, count);
                sendMailList.Add(sandNewMail);
            }

            return new Tuple<ErrorCode, List<Mail>>(ErrorCode.None, sendMailList);
        }
        catch
        {
            _logger.ZLogError($"[PayInAppDb.InsertReceiptDataAsync] ErrorCode : {ErrorCode.ReceiptException}");

            return new Tuple<ErrorCode, List<Mail>>(ErrorCode.ReceiptException, null);

        }
    }

    public async Task<Mail> SandInAppMailAsync(Int64 uniquekey, Int32 itemCode, Int64 count)
    {
        try
        {
            Item item = MasterDataDb.itemList.FirstOrDefault(i => i.code == itemCode);

            Mail newmail = new Mail
            {
                player_id = uniquekey,
                title = "[인앱결제] 인앱결제 아이템 지급",
                content = $"안녕하세, 인앱 결제한 아이템 : {item.name} 수량 : {count} 개를 지급합니다. 받기 버튼을 클릭하여 수령하세요 !",
                exp = 100, // 아이템 지급 우편 유효기간 
            };

            Console.WriteLine($"Mail exp : {newmail.exp}, title :{newmail.title}, content : {newmail.content}, type :{newmail.type}");

            GetInAppItem getInAppItem = new GetInAppItem
            {
                Code = item.code,
                Name = item.name,
                Count = count,
            };

            UserItemDb.playerMailInAppItemLsit.Add(getInAppItem);

            return newmail;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, "[PayInAppDb.SandInAppMailAsync] Failed to send Mail");

            return null;
        }
    }

    private async Task<Int32> GetCountByReceiptNoAsync(Int64 receiptno)
    {
        var query = new Query(GameDbTable.inapp).Where("receipt_number", receiptno).AsCount();
        var result = await _queryFactory.ExecuteScalarAsync<int>(query);

        return result;
    }

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.GameDb);
        _dbConn.Open();
    }

    private void Close() => _dbConn.Close();

    public void Dispose() => Close();
}