using System.Data;
using GameAPIServer.DBModel;
using GameAPIServer.StateType;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata;
using SqlKata.Execution;
using ZLogger;

namespace GameAPIServer.Services;

public class MailDb : IMailDb
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<MailDb> _logger;

    private IDbConnection _dbConn;
    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    public static Int32 pageNumber;
    public static List<Mail> mailList;

    public MailDb(ILogger<MailDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<ErrorCode> InsertMailAsync(Mail newMail, MailType mailType)
    {
        try
        {
            var query = await _queryFactory.Query(GameDbTable.mail).InsertAsync(new
            {
                player_id = newMail.player_id,
                title = newMail.title,
                content = newMail.content,
                expiration_date = newMail.exp,
                type = mailType,
                state_type = MailStateType.NotRead,
                item_code = newMail.item_code,
                count = newMail.count
            });

            if(query != 1)
            {
                _logger.ZLogError($"[MailDb.InsertMailAsync] ErrorCode : {ErrorCode.CreateMailFail}");
                return ErrorCode.CreateMailFail;
            }

            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e, $"[MailDb.InsertMailAsync] ErrorCode : {ErrorCode.CreateMailFailException}");
            return ErrorCode.CreateMailFailException;
        }
    }

    public async Task<ErrorCode> LoadMailAsync(Int64 userId, Int32 pageNum)
    {
        try
        {
            var pageSize = 20;
            var startRow = (pageNum - 1) * pageSize;

            var totalQuery = _queryFactory.Query(GameDbTable.mail).Where(DbColumn.player_id, userId);

            var queryCount = totalQuery.Clone().AsCount();

            var totalCount = await _queryFactory.ExecuteScalarAsync<int>(queryCount);
            var totalPages = totalCount / pageSize;

            if(totalCount % pageSize > 0)
            {
                totalPages++;
            }

            if (pageNum > totalPages)
            {
                return ErrorCode.MailPageIndexOutException;
            }

            var query = await _queryFactory.Query(GameDbTable.mail)
                .Select(DbColumn.player_id, "code", "title", "content", "exp", "type", "state_Type","item_code", "count", "created_at")
                .OrderByDesc(DbColumn.code)
                .Offset(startRow)
                .Limit(pageSize).GetAsync<Mail>();
            
            var curtime = DateTime.Now.Day;
            int index = 0;

            foreach(var mail in query)
            {
                var orginExp = mail.exp;
                var createdat = mail.created_at.Day;
                var exp = mail.exp;

                var checkday = curtime - createdat;

                if (checkday <= 0)
                {
                    exp -= 1;
                    mail.exp = exp;
                }

                mailList = query.ToList();

                if(exp == 0 || mail.type.Equals(MailStateType.Delete))
                {
                    mailList.RemoveAt(index);
                    ChangedMailTypeAsync(index);
                }

                index += 1;
            }

            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[MailDb.LoadPageMail] ErrorCode : {ErrorCode.LoadMailDataFailException}, player_id : {userId}");

            return ErrorCode.LoadMailDataFailException;
        }
    }

    public async Task<Tuple<MailStateType, String>> ReadMailContent(Int32 index)
    {
        try
        {
            var mail = mailList[index];

            if (mail == null || index > mailList.Count())
            {
                _logger.ZLogError(
                  $"[MailDb.ReadMailContent] ErrorCode : {ErrorCode.OpenMailDataFailException}, MailIndex : {index}");

                return new Tuple<MailStateType, String>(MailStateType.NotRead, null);
            }

            return new Tuple<MailStateType, String>(MailStateType.Read, mail.content);
        }
        catch (Exception e)
        {
            _logger.ZLogError(
                $"[MailDb.ReadMailContent] ErrorCode : {ErrorCode.OpenMailDataFailException}, MailListCout : {mailList.Count}");

            return new Tuple<MailStateType, String>(MailStateType.NotRead, null);
        }
    }

    private async Task<ErrorCode> ChangedExp(Int64 userId, Int64 exp)
    {
        try
        {
            var query = await _queryFactory.Query(GameDbTable.mail).Where(DbColumn.player_id, userId).UpdateAsync(new { exp = exp });

            if(query == 0)
            {
                return ErrorCode.UpdateExpFail;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[MailDb.ChangedExp] ErrorCode : {ErrorCode.UpdateExpFailException}, exp : {exp}");

            return ErrorCode.UpdateExpFailException;
        }
    }

    public async Task<ErrorCode> ChangedMailTypeAsync(Int32 index)
    {
        try
        {
            var openMailInfo = mailList[index];

            var player_id = openMailInfo.player_id;
            var code = openMailInfo.code;

            var query = await _queryFactory.Query(GameDbTable.mail).Where(DbColumn.player_id, player_id)
                .Where("code", code).UpdateAsync(new { state_type = MailStateType.Read});

            return ErrorCode.None;
        }
        catch
        {
            _logger.ZLogError(
              $"[MailDb.ChangedMailTypeAsync] ErrorCode : {ErrorCode.UpdatMailStateTypeException}, MailStateType : {mailList.Count}");

            return ErrorCode.UpdatMailStateTypeException;
        }
    }

    public List<Mail> GetMailList() => mailList;

    public Int32 GetItemCode(int index) => mailList[index].item_code;
    public Int64 GetItemCount(int index) => mailList[index].count;

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.GameDb);

        _dbConn.Open();
    }

    private void Close() => _dbConn.Close();

    private void Dispos() => Close();
}