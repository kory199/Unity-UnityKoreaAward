using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class PkEnhanceRequest
{
    [Required] public String Id { get; set; }
    [Required] public String AuthToken { get; set; }
    [Required] public Int32 ItemCode { get; set; }
    [Required] public Int32 EnhanceCount { get; set; }
}