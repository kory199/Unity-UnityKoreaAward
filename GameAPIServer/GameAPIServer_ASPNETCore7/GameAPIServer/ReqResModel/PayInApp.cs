using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class PkPayInAppRequest
{
    [Required] public String Id { get; set; }
    [Required] public String AuthToken { get; set; }
    [Required] public Int64 ReceipteNo { get; set; }
    [Required] public Int32 Code { get; set; } 
}