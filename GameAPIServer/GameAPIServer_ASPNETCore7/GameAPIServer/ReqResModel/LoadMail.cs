using System.ComponentModel.DataAnnotations;
using GameAPIServer.DBModel;

namespace GameAPIServer.ReqResModel;

public class PkLoadMailRequest
{
    [Required] public String Id  { get; set; }
    [Required] public String AuthToken { get; set; }
    [Required] public Int32 PageNum { get; set; } = 1;
}

public class PkLoadMailResPonse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public List<LoadMail> mailList { get; set; } = new List<LoadMail>();
}