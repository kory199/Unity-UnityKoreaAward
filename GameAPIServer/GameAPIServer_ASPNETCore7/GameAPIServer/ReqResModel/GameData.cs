using System;
using GameAPIServer.DBModel;
using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class PkLoadGameResPonse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public List<LoadMail> mailList { get; set; } = new List<LoadMail>();
}