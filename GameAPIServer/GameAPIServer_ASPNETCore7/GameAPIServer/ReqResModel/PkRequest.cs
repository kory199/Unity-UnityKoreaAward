using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class PkRequest
{
    [Required] public String Id { get; set; }
    [Required] public String AuthToken { get; set; }
}