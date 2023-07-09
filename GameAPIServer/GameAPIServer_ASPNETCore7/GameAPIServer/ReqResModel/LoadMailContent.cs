using System;
using System.ComponentModel.DataAnnotations;
using GameAPIServer.DBModel;

namespace GameAPIServer.ReqResModel;

public class PkLoadMailContentRequest
{
    [Required] public String Id { get; set; }
    [Required] public String AuthToken { get; set; }
    [Required] public Int32 PageNum { get; set; }
    [Required] public Int32 PageIndex { get; set; }
}

public class PkLoadMailContentResPonse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public ReadMailContent MailContentList { get; set; } = new ReadMailContent();
}