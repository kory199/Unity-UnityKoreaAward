using GameAPIServer.DBModel;
using GameAPIServer.StateType;

namespace GameAPIServer.Services;

public interface IMailDb
{
    public Task<ErrorCode> LoadMailAsync(Int64 uniqueKey, Int32 pageNum);

    public Task<Tuple<MailStateType, String>> ReadMailContent(Int32 index);

    public Task<ErrorCode> ChangedMailTypeAsync(Int32 index);

    public Task<ErrorCode> InsertMailAsync(Mail newMail, MailType mailType);

    public List<Mail> GetMailList();

    public Int32 GetItemCode(int index);
    public Int64 GetItemCount(int index);
}